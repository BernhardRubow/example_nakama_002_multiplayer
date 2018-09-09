using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets._sts.scripts.messages;
using UnityEngine;

namespace Assets._sts.scripts
{


    public class stsFireRocket : MonoBehaviour, IRemoteMessageHandler, ILocalPlayerAwareScript
    {
        // +++ public fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public bool IsLocalPlayer;




        // +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private nvpGame _Game;
        private nvpMultiplayerManager _MultiplayerManager;
        private Rigidbody _Body;
        private IMissleLauncher _missleLauncher;




        // +++ unity life cycle +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        void Start()
        {
            Init();
        }

        void Update()
        {
            if (!IsLocalPlayer) return;

            if (Input.GetButtonDown("Fire1") || Input.GetKeyUp(KeyCode.F))
            {
                var direction = _Body.velocity;
                CreateRocket(direction);
                _MultiplayerManager.SendRocket(_Body.position, direction);
            }
        }




        // +++ interface methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void HandleMessage(object msg)
        {
            if (msg is RocketFiredMessage)
            {
                var positionUpdateMessage = (RocketFiredMessage)msg;
                CreateRocket(positionUpdateMessage.ToDirection());
            }
        }




        // +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void CreateRocket(Vector3 direction)
        {
            _missleLauncher.LaunchMissle(direction);
        }

        private void Init()
        {
            _Body = this.GetComponent<Rigidbody>();
            _MultiplayerManager = GameObject.Find("sceneManagers").GetComponent<nvpMultiplayerManager>();
            _missleLauncher = this.GetComponentInChildren<IMissleLauncher>();
        }

        public void SetIsLocalPlayer()
        {
            IsLocalPlayer = true;
        }
    }

}
