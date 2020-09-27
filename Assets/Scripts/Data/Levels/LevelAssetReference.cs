using System;
using UnityEngine.AddressableAssets;

namespace TD.Data.Levels
{
    [Serializable]
    public class LevelAssetReference : AssetReferenceT<Level>
    {
        public LevelAssetReference(string guid) : base(guid)
        {
        }
    }
}