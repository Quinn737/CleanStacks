
using System.Collections;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace CleanStacks
{
    internal static class StackCountText
    {
        internal static void HideMaximum(object component)
        {
            if (component == null)
                return;

            // Search silently instead of using AccessTools.Property,
            // which logs warnings when a component has no text property.
            PropertyInfo property = component.GetType().GetProperty(
                "text",
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            if (property == null ||
                !property.CanRead ||
                !property.CanWrite ||
                property.PropertyType != typeof(string))
                return;

            string text = property.GetValue(component) as string;

            if (string.IsNullOrEmpty(text))
                return;

            int slash = text.IndexOf('/');

            if (slash > 0)
                property.SetValue(
                    component, text.Substring(0, slash));
        }
    }

    [HarmonyPatch(typeof(InventoryGrid), "UpdateGui")]
    internal static class InventoryGrid_UpdateGui_Patch
    {
        private static void Postfix(InventoryGrid __instance)
        {
            if (__instance == null)
                return;

            IList elements = AccessTools.Field(
                typeof(InventoryGrid), "m_elements")
                ?.GetValue(__instance) as IList;

            if (elements == null)
                return;

            foreach (object element in elements)
            {
                if (element == null)
                    continue;

                object amount = AccessTools.Field(
                    element.GetType(), "m_amount")
                    ?.GetValue(element);

                StackCountText.HideMaximum(amount);
            }
        }
    }

    [HarmonyPatch(typeof(Hud), "Update")]
    internal static class Hud_Update_Patch
    {
        private static void Postfix(Hud __instance)
        {
            if (__instance == null)
                return;

            foreach (Transform transform in
                __instance.GetComponentsInChildren<Transform>(true))
            {
                if (transform == null ||
                    string.IsNullOrEmpty(transform.name) ||
                    !transform.name.ToLowerInvariant().Contains("hotkey"))
                    continue;

                foreach (Component component in
                    transform.GetComponentsInChildren<Component>(true))
                {
                    StackCountText.HideMaximum(component);
                }
            }
        }
    }
}
