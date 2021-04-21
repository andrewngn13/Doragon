using UnityEngine;

namespace Doragon.Mapping
{
    public class MappingCore
    {
        // map size of 8x8
        private GameObject[,] mappedObjects;

        // TODO: handle loading of saved mapped objects
        public MappingCore()
        {
            mappedObjects = new GameObject[8, 8];
        }

        // TODO: snap object to grid, save position, and delete previous if needed

        // TODO: Save mapped objects to drive
    }
}
