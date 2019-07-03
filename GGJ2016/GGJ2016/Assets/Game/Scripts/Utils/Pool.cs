﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Game {

	public interface IPool {
		void Add(GameObject pool);
		void Remove(GameObject pool);
		GameObject Get();
		GameObject Tempalte();
	};

	public class Pool : MonoBehaviour, IPool {

		[SerializeField]
		private GameObject template;

		[SerializeField]
		private List<GameObject> activePool;

		[SerializeField]
		private List<GameObject> inActivePool;
		
		private Dictionary<GameObject, bool> poolMap;

		private void Awake() {
			Assertion.AssertNotNull(this.template);
			this.poolMap = new Dictionary<GameObject, bool>();
		}

		private void Start() {
			foreach (GameObject pool in this.activePool) {
				this.poolMap[pool] = true;
			}

			foreach (GameObject pool in this.inActivePool) {
				this.poolMap[pool] = false;
			}
		}

		private void OnDestroy() {
		}

		private GameObject Create() {
			// TODO: Pool the bubbles here
			Transform template = this.template.transform;

			GameObject bubble = (GameObject)GameObject.Instantiate(template.gameObject, template.position, template.rotation);
			bubble.transform.SetParent(template.parent);
			bubble.transform.localPosition = template.localPosition;
			bubble.transform.localScale = template.localScale;
			bubble.SetActive(true);

			return bubble;
		}

		private void AddToMap(GameObject pool, bool isActive) {
			if (this.poolMap.ContainsKey(pool)) {
				this.poolMap[pool] = isActive;
			}
			else {
				this.poolMap.Add(pool, isActive);
			}
		}

		public void Add(GameObject pool) {
			//if (this.activePool.Exists(p => p.Equals(pool))) {
			//	this.activePool.Remove(pool);
			//}

			if (!this.activePool.Exists(p => p.Equals(pool))) {
				this.activePool.Add(pool);
			}

			if (this.inActivePool.Exists(p => p.Equals(pool))) {
				this.inActivePool.Remove(pool);
			}

			//this.activePool.Add(pool);
			this.AddToMap(pool, true);
			pool.transform.position = this.template.transform.position;
			pool.SetActive(true);
		}

		public void Remove(GameObject pool) {
			if (this.activePool.Exists(p => p.Equals(pool))) {
				this.activePool.Remove(pool);
			}

			//if (this.inActivePool.Exists(p => p.Equals(pool))) {
			//	this.inActivePool.Remove(pool);
			//}

			if (!this.inActivePool.Exists(p => p.Equals(pool))) {
				this.inActivePool.Add(pool);
			}

			//this.inActivePool.Add(pool);
			this.AddToMap(pool, false);
			pool.transform.position = this.template.transform.position;
			pool.SetActive(false);
		}

		public GameObject Get() {
			/*
			if (this.inActivePool.Count <= 0) {
				return this.Create();
			}

			foreach (GameObject pool in this.inActivePool) {
				if (!this.poolMap.ContainsKey(pool)) {
					Assertion.Assert(false);
				}

				if (!this.poolMap[pool]) {
					return pool;
				}
			}

			Assertion.Assert(false);
			return null;
			*/

			return this.Create();
		}

		public GameObject Tempalte() {
			return this.template;
		}

	}

}