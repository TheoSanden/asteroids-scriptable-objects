using UnityEditor.VersionControl;
using System.Collections;
using System;
using UnityEngine;
using Variables;

namespace Ship
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Engine : MonoBehaviour
    {
        [SerializeField] private FloatVariable _throttlePower;
        [SerializeField] private AnimationCurve dashCurve;
        [SerializeField] private FloatVariable _strafePower;
        [SerializeField] private FloatVariable _dashPower;
        [SerializeField] private float _throttlePowerSimple;
        [SerializeField] private float _rotationPowerSimple;
        [SerializeField, Range(0.0f, 1.0f)] private float maxReverseAccelleration;
        [SerializeField] private AudioClip dashClip;
        [SerializeField] private DashTrail trail;
        private Rigidbody2D _rigidbody;
        private AudioSource _audioSource;
        int dashBuffer = 0;
        bool dashing = false;
        bool additionalDash = false;
        private void FixedUpdate()
        {
            UpdateLookDirection();
            if (Input.GetAxis("Vertical") > 0)
            {
                if (((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).magnitude < 10) { return; }
                Throttle(1);

            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = dashClip;
        }

        public void Throttle(int sign)
        {

            _rigidbody.AddForce(transform.up * sign * _throttlePower.Value * GetReverseMultiplier(transform.up), ForceMode2D.Force);
        }
        public void Dash()
        {
            if (dashBuffer >= 2) { return; }
            if (dashBuffer < 2 && dashing)
            {
                additionalDash = true;
                StartCoroutine(CoroutineHelper.SetAfterSeconds(result => additionalDash = result, false, 0.3f));
                return;
            }
            if (dashBuffer == 0) StartCoroutine(CoroutineHelper.SetAfterSeconds(result => dashBuffer = result, 0, 1.2f));
            dashBuffer++;
            StartCoroutine(Dash((Vector2)transform.position + (GetMouseDirection() * _dashPower.Value), 0.2f));
        }
        public void UpdateLookDirection()
        {
            transform.up = GetMouseDirection();
        }
        private float GetReverseMultiplier(Vector2 moveTo)
        {
            Vector2 moveDirection = _rigidbody.velocity;
            float reverseMultiplier = Vector2.Dot(moveTo.normalized, moveDirection.normalized);
            reverseMultiplier = 1 + ((1 - reverseMultiplier) / 2) * maxReverseAccelleration;
            return reverseMultiplier;
        }
        private IEnumerator Dash(Vector2 position, float time)
        {
            trail.Play(this.transform, time, 40);
            _audioSource.clip = dashClip;
            _audioSource.Play();
            dashing = true;
            Vector2 path = (Vector2)position - (Vector2)transform.position;
            Vector2 originalPosition = transform.position;
            float timer = 0;
            Vector2 positionLastFrame = Vector2.zero;
            while (timer <= time)
            {
                positionLastFrame = transform.position;
                _rigidbody.velocity = Vector2.zero;
                transform.position = (Vector3)originalPosition + (Vector3)(path * dashCurve.Evaluate(timer / time));
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if (additionalDash) { dashing = false; additionalDash = false; Dash(); }
            else
            {
                _rigidbody.velocity = (path.normalized * Mathf.Abs(((Vector2)transform.position - positionLastFrame).magnitude)) / Time.deltaTime;
                dashing = false;
            }
        }
        private Vector2 GetMouseDirection()
        {

            return ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
        }
    }
}
