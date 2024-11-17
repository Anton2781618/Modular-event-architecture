// using NaughtyAttributes;
using UnityEngine;

namespace Code.Global.UnitSystem.Base.Components
{
    public class SkinnedMeshRendererAttachment : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer attachedSkinnedMeshRenderer;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRendererToAttach;
        
        [Header("Bones")]
        [SerializeField] private Transform rootBone;
        [SerializeField] private Transform[] bones;
       
        private const string AttachmentMethodTitle = "Attach Skinned Mesh Renderer";
        private const string CacheBonesMethodTitle = "Cache Bones";

        [ContextMenu(CacheBonesMethodTitle)]
        // [Button(CacheBonesMethodTitle)]
        private void CacheBones()
        {
            rootBone = attachedSkinnedMeshRenderer.rootBone;
            bones = attachedSkinnedMeshRenderer.bones;
        }
        
        [ContextMenu(AttachmentMethodTitle)]
        // [Button(AttachmentMethodTitle)]
        private void Attach()
        {
            Attach(skinnedMeshRendererToAttach);
        }

        public void Attach(SkinnedMeshRenderer toAttach)
        {
            toAttach.rootBone = rootBone;
            toAttach.bones = bones;
        }
    }
}