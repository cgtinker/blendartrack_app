using UnityEngine;
namespace UX
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
            Viewer,
            Settings
        }

        private State _uiState;

        public void SetState(State state)
        {
            _uiState = state;
            UpdateState();
        }

        private void Awake()
        {
            Debug.Log("test");
        }

        private void UpdateState()
        {
            switch (_uiState)
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

                case State.Viewer:
                    break;

                case State.Settings:
                    break;

                default:
                    break;
            }
        }
    }
}

