using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemiesSystem
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private MovableEntity entity;
        [SerializeField] private float rotateCooldown;

        private readonly MoveDirection[] _moveDirections =
            { MoveDirection.UP, MoveDirection.RIGHT, MoveDirection.DOWN, MoveDirection.LEFT };

        private bool _isRotateCooldown;

        private void Update()
        {
            var result = entity.Move();
            
            if(_isRotateCooldown)
                return;
            
            var exceptDir = entity.CurrentDirection switch
            {
                MoveDirection.UP => MoveDirection.DOWN,
                MoveDirection.RIGHT => MoveDirection.LEFT,
                MoveDirection.DOWN => MoveDirection.UP,
                MoveDirection.LEFT => MoveDirection.RIGHT,
                _ => throw new ArgumentOutOfRangeException()
            };

            var newDirections = _moveDirections.Except(new[] { exceptDir }).ToList();
            
            var toRotate = newDirections.Where(direction => entity.CanRotate(direction)).ToList();

            var rndRotate = Random.Range(0, toRotate.Count + 1);

            if (toRotate.Count > 0 && rndRotate != 0)
            {
                entity.Rotate(toRotate[rndRotate - 1]);
                StartCoroutine(RotateCooldownCoroutine());
                Debug.Log(toRotate[rndRotate - 1].ToString());
                return;
            }

            
            if (result)
                return;

            var moveDirections = _moveDirections.Except(new[] { entity.CurrentDirection }).ToList();
            entity.Rotate(moveDirections[Random.Range(0, moveDirections.Count)]);
            StartCoroutine(RotateCooldownCoroutine());
        }

        private IEnumerator RotateCooldownCoroutine()
        {
            _isRotateCooldown = true;
            
            yield return new WaitForSeconds(rotateCooldown);
            
            _isRotateCooldown = false;
        }
    }
}