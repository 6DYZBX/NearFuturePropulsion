﻿// Utils
// ---------------------------------
// Static functions that are useful for the NearFuture pack

using System;
using UnityEngine;
using System.Collections.Generic;

namespace NearFuturePropulsion
{
    internal static class Utils
    {
    

        // This function loads up some animationstates
        public static AnimationState[] SetUpAnimation(string animationName, Part part)
        {
            var states = new List<AnimationState>();
            int layer = 0;
            foreach (var animation in part.FindModelAnimators(animationName))
            {
                var animationState = animation[animationName];
                animationState.speed = 0;
                animationState.enabled = true;
                // Clamp this or else weird things happen
                animationState.wrapMode = WrapMode.ClampForever;
                animation.Blend(animationName);
                animationState.blendMode = AnimationBlendMode.Additive;
                // animationState.layer = layer;
                states.Add(animationState);
                layer++;
            }
            // Convert 
            return states.ToArray();
        }

        // Returns true if ship is it atmoshpere
        public static bool VesselInAtmosphere(Vessel vessel)
        {
           return vessel.heightFromSurface < vessel.mainBody.maxAtmosphereAltitude;
        }

    
        // fix for deprecated Unity function
        public static void SetActiveRecursively(GameObject obj, bool active)
        {
           obj.SetActive(active);
 
           foreach (Transform child in obj.transform)
           {
                SetActiveRecursively(child.gameObject, active);
           }
        }
        // Converts to a time string from a seconds, accounting for kerbal time
        public static string FormatTimeString(double seconds)
        {
            double dayLength;
            double rem;
            if (GameSettings.KERBIN_TIME)
                dayLength = 6d;
            else
                dayLength = 24d;

            int years = (int)(seconds / (3600.0d * dayLength * 365.0d));
            rem = seconds % (3600.0d * dayLength * 365.0d);
            int days = (int) (rem / (3600.0d * dayLength));
            rem = rem % (3600.0d * dayLength);
            int hours = (int)(rem / (3600.0d));
            rem = rem % (3600.0d);
            int minutes = (int)(rem / (60.0d));
            int secs = (int)rem;

            string result = "";

            // draw years + days
            if (years > 0)
            {
                result += years.ToString() + "y ";
                result += days.ToString() + "d ";
                result += hours.ToString() + "h";
                result += minutes.ToString() + "m";
            }else if ( days > 0)
            {
                result += days.ToString() + "d ";
                result += hours.ToString() + "h";
                result += minutes.ToString() + "m";
                result += secs.ToString() + "s";
            } else if ( hours > 0)
            {
                result += hours.ToString() + "h ";
                result += minutes.ToString() + "m ";
                result += secs.ToString() + "s";
            } else if (minutes > 0)
            {
                result += minutes.ToString() + "m ";
                result += secs.ToString() + "s";
            }
            else if (seconds > 0)
            {
                result += secs.ToString() + "s";
            }
            else
            {
                result = "None";
            }
          

            return result;
        }

        // finds the flow rate given thrust, isp and the propellant 
        public static float FindFlowRate(float thrust, float isp, Propellant fuelPropellant)
        {
            double fuelDensity = PartResourceLibrary.Instance.GetDefinition(fuelPropellant.name).density;
            double fuelRate = ((thrust * 1000f) / (isp * 9.82d)) / (fuelDensity * 1000f);
            return (float)fuelRate;
        }

        public static float FindFlowRate(float thrust, float isp, string fuelPropellant)
        {
            double fuelDensity = PartResourceLibrary.Instance.GetDefinition(fuelPropellant).density;
            double fuelRate = ((thrust * 1000f) / (isp * 9.82d)) / (fuelDensity * 1000f);
            return (float)fuelRate;
        }

        public static float FindIsp(float thrust, float flowRate, Propellant fuelPropellant)
        {
            double fuelDensity = PartResourceLibrary.Instance.GetDefinition(fuelPropellant.name).density;
            double isp = (((thrust * 1000f) / (9.82d)) / flowRate) / (fuelDensity * 1000f);
            return (float)isp;
        }
        public static float FindIsp(float thrust, float flowRate, string fuelPropellant)
        {
            double fuelDensity = PartResourceLibrary.Instance.GetDefinition(fuelPropellant).density;
            double isp = (((thrust * 1000f) / (9.82d)) / flowRate) / (fuelDensity * 1000f);
            return (float)isp;
        }

        public static float FindPowerUse(float thrust,float isp,Propellant ecPropellant, Propellant fuelPropellant)
        {
          
            return (ecPropellant.ratio / fuelPropellant.ratio) * FindFlowRate(thrust, isp, fuelPropellant);
        }
    }

    
    public enum RadiatorState
    {
        Deployed,
        Deploying,
        Retracted,
        Retracting,
        Broken,
    }
}
