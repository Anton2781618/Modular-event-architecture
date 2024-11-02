using UnityEngine;

public class PlayerMovementSystem
{
        private Transform _bodyTransform;
        private Transform _cameraTransform;
        private Animator _animator;

        private Vector3 _camForwarod;
        public Vector3 MoveVector {get; private set;}
        private Vector3 _moveInput;
        private float _forwardAmount;
        private float _turnAmount;

        public bool Sprint {get; set;} = false;

        public PlayerMovementSystem(Animator animator)
        {
            _animator = animator;

            _bodyTransform = _animator.transform;

            _cameraTransform = Camera.main.transform;
        }

        public void UpdateMe()
        {
            Moving();

            RotateToTarget();
        }
        
        private void RotateToTarget()
        {
            Vector3 mouseWorldPosition = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 50f))
            {
                mouseWorldPosition = hit.point;
            }
            
            Vector3 worldAimTarget = mouseWorldPosition;

            worldAimTarget.y = _bodyTransform.position.y;
            
            Vector3 aimDirection = (worldAimTarget - _bodyTransform.position).normalized;

            _bodyTransform.forward = Vector3.Lerp(_bodyTransform.forward, aimDirection, Time.deltaTime * 10f);
        }
        


        //определить поворот персонажа в зависимости от ввода
        private void Moving()
        {
            float horizontal = Input.GetAxis("Horizontal");

            float vertical = Input.GetAxis("Vertical");

            _camForwarod = Vector3.Scale(_cameraTransform.up, new Vector3(1, 0, 1)).normalized;

            MoveVector = vertical * _camForwarod + horizontal * _cameraTransform.right;

            if(MoveVector.magnitude > 1f) MoveVector.Normalize();

            Move(MoveVector);
        }

        //перемещение персонажа
        private void Move(Vector3 move)
        {
            if(move.magnitude > 1f) move.Normalize();

            _moveInput = move;

            ConvertMoveInput();

            UpdateAnimator();
        }

        //преобразование ввода перемещения в зависимости от камеры
        private void ConvertMoveInput()
        {
            Vector3 localMove = _bodyTransform.InverseTransformDirection(_moveInput);

            _turnAmount = localMove.x;

            _forwardAmount = localMove.z;
        }

        public float trr = 3f;
        [SerializeField] private float maxTension = 5f; // Максимальное напряжение веревки
        [SerializeField] private float minSpeedMultiplier = 0.2f; // Минимальный множитель скорости


    //обновление аниматора
    private void UpdateAnimator()
    {
        _animator.SetFloat("Forward", Sprint ? _forwardAmount * 2 : _forwardAmount, 0.1f, Time.deltaTime);

        _animator.SetFloat("Turn", _turnAmount, 0.1f, Time.deltaTime);

    }
}
