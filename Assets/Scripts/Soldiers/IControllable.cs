using UnityEngine;

namespace Raiders
{
    public interface IControllable
    {
        public IControllable mainControllable {get;}

        public void GoTo(Vector3 destination);
    }
}
