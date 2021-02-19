using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class UIStateMachine : Singleton<UIStateMachine>
    {
        public enum State
        {
            StartUp,
            Update,
            Tutorial,
            Tracking,
            Filebrowser,
            Settings
        }

        private State uiState;

        public void SetState(State state)
        {
            uiState = state;
            UpdateState();
        }

        private void UpdateState()
        {
            switch (uiState)
            {
                case State.StartUp:
                    break;

                case State.Update:
                    break;

                case State.Tutorial:
                    break;

                case State.Tracking:
                    break;

                case State.Filebrowser:
                    break;

                case State.Settings:
                    break;

                default:
                    break;
            }
        }
    }
}

