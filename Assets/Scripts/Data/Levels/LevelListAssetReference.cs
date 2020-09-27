using System;
using UnityEngine.AddressableAssets;

namespace TD.Data.Levels
{
    [Serializable]
    public class LevelListAssetReference : AssetReferenceT<LevelList>
    {
        public LevelListAssetReference(string guid) : base(guid)
        {
        }
    }
}