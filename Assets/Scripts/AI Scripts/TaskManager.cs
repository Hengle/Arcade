using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager {
	
	Queue<Task> taskQueue;

	public TaskManager () {
		taskQueue = new Queue<Task> ();
	}

	Task GetTaskFromQueue () {
		return null;
	}

	public class Task {

		public Task () {

		}
	}
}
