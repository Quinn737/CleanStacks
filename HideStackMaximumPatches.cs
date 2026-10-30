using HarmonyLib;
using TMPro;
using UnityEngine;

namespace CleanStacks
{
    internal static class StackCountText
    {
        internal static void HideMaximum(TMP_Text text)
        {
            if (text == null || string.IsNullOrEmpty(text.text))
                return;

            // Handles both "12/50" and "12 / 50"
            int slash = text.text.IndexOf('/');
            if (slash > 0)
            {
                text.text = text.text.Substring(0, slash).TrimEnd();
            }
        }
    }

    // ========== Hotbar ==========
    [HarmonyPatch(typeof(HotkeyBar), "UpdateIcons")]
    internal static class HotkeyBar_UpdateIcons_Patch
    {
        private static void Postfix(HotkeyBar __instance)
        {
            if (__instance == null || __instance.m_elements == null)
                return;

            foreach (var element in __instance.m_elements)
            {
                if (element != null && element.m_amount != null && element.m_amount.gameObject.activeSelf)
                {
                    StackCountText.HideMaximum(element.m_amount);
                }
            }
        }
    }

    // ========== Inventory & Chests ==========
    [HarmonyPatch(typeof(InventoryGrid), "UpdateGui")]
    internal static class InventoryGrid_UpdateGui_Patch
    {
        private static void Postfix(InventoryGrid __instance)
        {
            if (__instance == null)
                return;

            // m_elements is a List<InventoryElement>
            var elements = __instance.m_elements;
            if (elements == null)
                return;

            foreach (var element in elements)
            {
                if (element != null && element.m_amount != null && element.m_amount.enabled)
                {
                    StackCountText.HideMaximum(element.m_amount);
                }
            }
        }
    }
}