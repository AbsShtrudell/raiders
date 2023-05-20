using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Raiders
{
    public class Slot : INetworkSerializable
    {
        private float _progress = 0f;
        private bool _blocked = false;

        public Slot()
        { }
        public Slot(float progress)
        {
            Progress = progress;
        }

        public float Progress { get { return _progress; } private set { _progress = Mathf.Clamp(value, 0f, 1f); } }
        public bool IsBlocked { get { return _blocked; } set { _blocked = value; } }
        public bool IsFull { get { return _progress == 1f; } }
        public bool IsEmpty { get { return _progress == 0f; } }

        public void Fill()
        {
            Progress = 1f;
        }

        public void Empty()
        {
            Progress = 0f;
        }

        public float Recover(float speed)
        {
            return Progress = _progress + (speed * Time.deltaTime);
        }

        public float Decay(float speed)
        {
            return Progress = _progress - (speed * Time.deltaTime); ;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _progress);
            serializer.SerializeValue(ref _blocked);
        }
    }
}
