using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VolumetricFogAndMist2 {

    [ExecuteInEditMode]
    public class PointLightManager : MonoBehaviour, IVolumetricFogManager {

        public string managerName {
            get {
                return "Point Light Manager";
            }
        }

        public const int MAX_POINT_LIGHTS = 16;

        [Header("Point Light Search Settings")]
        [Tooltip("Point lights are sorted by distance to tracking center object")]
        public Transform trackingCenter;
        public float newLightsCheckInterval = 3f;
        [Tooltip("Excludes lights behind camera")]
        public bool excludeLightsBehind = true;

        [Header("Common Settings")]
        [Tooltip("Global inscattering multiplier for point lights")]
        public float inscattering = 1f;
        [Tooltip("Global intensity multiplier for point lights")]
        public float intensity = 1f;
        [Tooltip("Reduces light intensity near point lights")]
        public float insideAtten;

        Light[] pointLights;
        Vector4[] pointLightColorBuffer;
        Vector4[] pointLightPositionBuffer;
        float checkNewLightsLastTime;

        private void OnEnable() {
            if (trackingCenter == null) {
                Camera cam = null;
                Tools.CheckCamera(ref cam);
                if (cam != null) {
                    trackingCenter = cam.transform;
                }
            }
            if (pointLightColorBuffer == null || pointLightColorBuffer.Length != MAX_POINT_LIGHTS) {
                pointLightColorBuffer = new Vector4[MAX_POINT_LIGHTS];
            }
            if (pointLightPositionBuffer == null || pointLightPositionBuffer.Length != MAX_POINT_LIGHTS) {
                pointLightPositionBuffer = new Vector4[MAX_POINT_LIGHTS];
            }
        }

        private void LateUpdate() {
            TrackPointLights();
            SubmitPointLightData();
        }

        void SubmitPointLightData() {

            int k = 0;
            bool appIsRunning = Application.isPlaying;
            Vector3 trackingCenterPosition = trackingCenter.position;
            Vector3 trackingCenterForward = trackingCenter.forward;

            for (int i = 0; k < MAX_POINT_LIGHTS && i < pointLights.Length; i++) {
                Light light = pointLights[i];
                if (light == null || !light.isActiveAndEnabled || light.type != LightType.Point) continue;
                Vector3 pos = light.transform.position;
                float range = light.range;

                // if point light is behind camera and beyond the range, ignore it
                if (appIsRunning && excludeLightsBehind) {
                    Vector3 toLight = pos - trackingCenterPosition;
                    float dot = Vector3.Dot(trackingCenterForward, pos - trackingCenterPosition);
                    if (dot < 0 && toLight.sqrMagnitude > range * range) {
                        continue;
                    }
                }
                
                // add light to the buffer if intensity is enough
                range *= inscattering / 25f; // note: 25 comes from Unity point light attenuation equation
                float multiplier = light.intensity * intensity;

                if (range > 0 && multiplier > 0) {
                    pointLightPositionBuffer[k].x = pos.x;
                    pointLightPositionBuffer[k].y = pos.y;
                    pointLightPositionBuffer[k].z = pos.z;
                    pointLightPositionBuffer[k].w = 0;
                    Color color = light.color;
                    pointLightColorBuffer[k].x = color.r * multiplier;
                    pointLightColorBuffer[k].y = color.g * multiplier;
                    pointLightColorBuffer[k].z = color.b * multiplier;
                    pointLightColorBuffer[k].w = range;
                    k++;
                }
            }

            Shader.SetGlobalVectorArray(ShaderParams.PointLightColors, pointLightColorBuffer);
            Shader.SetGlobalVectorArray(ShaderParams.PointLightPositions, pointLightPositionBuffer);
            Shader.SetGlobalFloat(ShaderParams.PointLightInsideAtten, insideAtten);
            Shader.SetGlobalInt(ShaderParams.PointLightCount, k);
        }

        /// <summary>
        /// Look for nearest point lights
        /// </summary>
        public void TrackPointLights(bool forceImmediateUpdate = false) {

            // Look for new lights?
            if (forceImmediateUpdate || pointLights == null || !Application.isPlaying || (newLightsCheckInterval > 0 && Time.time - checkNewLightsLastTime > newLightsCheckInterval)) {
                checkNewLightsLastTime = Time.time;
                pointLights = FindObjectsOfType<Light>();
                System.Array.Sort(pointLights, pointLightsDistanceComparer);
            }
        }


        int pointLightsDistanceComparer(Light l1, Light l2) {
            float dist1 = (l1.transform.position - trackingCenter.position).sqrMagnitude;
            float dist2 = (l2.transform.position - trackingCenter.position).sqrMagnitude;
            if (dist1 < dist2) return -1;
            if (dist1 > dist2) return 1;
            return 0;
        }



    }

}