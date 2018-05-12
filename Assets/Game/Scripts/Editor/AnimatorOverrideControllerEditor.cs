using System;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
namespace UnityEditor
{

    [CanEditMultipleObjects, CustomEditor(typeof(AnimatorOverrideController))]
    internal class AnimatorOverrideControllerEditor : Editor
    {
        private static int sortType = 0;

        private SerializedProperty m_Controller;
        private AnimationClipPair[] m_Clips;
        private ReorderableList m_ClipList;
        private GUIStyle textStyle;

        private void OnEnable()
        {
            AnimatorOverrideController animatorOverrideController = this.target as AnimatorOverrideController;
            this.m_Controller = base.serializedObject.FindProperty("m_Controller");
            if (this.m_ClipList == null)
            {
                this.m_ClipList = new ReorderableList(animatorOverrideController.clips, typeof(AnimationClipPair), false, true, false, false);
                this.m_ClipList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawClipElement);
                this.m_ClipList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawClipHeader);
                this.m_ClipList.elementHeight = 16f;
            }
            AnimatorOverrideController expr_8B = animatorOverrideController;
            // expr_8B.OnOverrideControllerDirty = (AnimatorOverrideController.OnOverrideControllerDirtyCallback)Delegate.Combine(expr_8B.OnOverrideControllerDirty, new AnimatorOverrideController.OnOverrideControllerDirtyCallback(base.Repaint));
            textStyle = new GUIStyle();
            textStyle.normal.textColor = Color.white;
            textStyle.wordWrap = false;
            textStyle.richText = true;
        }

        private void OnDisable()
        {
            AnimatorOverrideController animatorOverrideController = this.target as AnimatorOverrideController;
            AnimatorOverrideController expr_0D = animatorOverrideController;
            // expr_0D.OnOverrideControllerDirty = (AnimatorOverrideController.OnOverrideControllerDirtyCallback)Delegate.Remove(expr_0D.OnOverrideControllerDirty, new AnimatorOverrideController.OnOverrideControllerDirtyCallback(base.Repaint));
        }

        public override void OnInspectorGUI()
        {
            bool flag = base.targets.Length > 1;
            bool flag2 = false;
            base.serializedObject.UpdateIfDirtyOrScript();
            AnimatorOverrideController animatorOverrideController = this.target as AnimatorOverrideController;
            RuntimeAnimatorController runtimeAnimatorController = (!this.m_Controller.hasMultipleDifferentValues) ? animatorOverrideController.runtimeAnimatorController : null;
            EditorGUI.BeginChangeCheck();
            runtimeAnimatorController = (EditorGUILayout.ObjectField("Controller", runtimeAnimatorController, typeof(UnityEditor.Animations.AnimatorController), false, new GUILayoutOption[0]) as RuntimeAnimatorController);
            sortType = EditorGUILayout.IntPopup("Sort type:", sortType, new string[] { "Default", "Original name", "Override name", "Changes" }, new int[] { 0, 1, 2, 3 });
            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < base.targets.Length; i++)
                {
                    AnimatorOverrideController animatorOverrideController2 = base.targets[i] as AnimatorOverrideController;
                    animatorOverrideController2.runtimeAnimatorController = runtimeAnimatorController;
                }
                flag2 = true;
            }
            EditorGUI.BeginDisabledGroup(this.m_Controller == null || (flag && this.m_Controller.hasMultipleDifferentValues) || runtimeAnimatorController == null);
            EditorGUI.BeginChangeCheck();
            this.m_Clips = animatorOverrideController.clips;
            switch (sortType)
            {
                case 1:
                    Array.Sort(this.m_Clips, (p1, p2) => {
                        return p1.originalClip.name.CompareTo(p2.originalClip.name);
                    });
                    break;
                case 2:
                    Array.Sort(this.m_Clips, (p1, p2) => {
                        if (p1.overrideClip == null)
                            return 1;
                        if (p2.overrideClip == null)
                            return -1;
                        return p1.overrideClip.name.CompareTo(p2.overrideClip.name);
                    });
                    break;
                case 3:
                    Array.Sort(this.m_Clips, (p1, p2) => {
                        if (p1.overrideClip == null)
                            return 1;
                        if (p2.overrideClip == null)
                            return -1;
                        if (p1.overrideClip != p1.originalClip && p2.overrideClip == p2.originalClip)
                            return -1;
                        if (p2.overrideClip != p2.originalClip && p1.overrideClip == p1.originalClip)
                            return 1;
                        return p1.overrideClip.name.CompareTo(p2.overrideClip.name);
                    });
                    break;
            }
            this.m_ClipList.list = this.m_Clips;
            this.m_ClipList.DoLayoutList();
            if (EditorGUI.EndChangeCheck())
            {
                for (int j = 0; j < base.targets.Length; j++)
                {
                    AnimatorOverrideController animatorOverrideController3 = base.targets[j] as AnimatorOverrideController;
                    animatorOverrideController3.clips = this.m_Clips;
                }
                flag2 = true;
            }
            EditorGUI.EndDisabledGroup();
            if (flag2)
            {
                // animatorOverrideController.PerformOverrideClipListCleanup();
            }
        }

        private void DrawClipElement(Rect rect, int index, bool selected, bool focused)
        {
            AnimationClip originalClip = this.m_Clips[index].originalClip;
            AnimationClip animationClip = this.m_Clips[index].overrideClip;
            rect.xMax /= 2f;
            var color = "black";
            if (animationClip == null)
                color = "red";
            else
            if (animationClip != originalClip)
                color = "green";
            GUI.Label(rect, "<color=" + color + ">" + originalClip.name + "</color>", textStyle);
            rect.xMin = rect.xMax;
            rect.xMax *= 2f;
            EditorGUI.BeginChangeCheck();
            animationClip = (EditorGUI.ObjectField(rect, string.Empty, animationClip, typeof(AnimationClip), false) as AnimationClip);
            if (EditorGUI.EndChangeCheck())
            {
                this.m_Clips[index].overrideClip = animationClip;
            }
        }

        private void DrawClipHeader(Rect rect)
        {
            rect.xMax /= 2f;
            GUI.Label(rect, "Original", EditorStyles.label);
            rect.xMin = rect.xMax;
            rect.xMax *= 2f;
            GUI.Label(rect, "Override", EditorStyles.label);
        }

    }

}