using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TNNUtils.Tween
{
    public class Tween : MonoBehaviour
    {
        public static Tween Instance;
        public List<Task> tasks;
        public List<Task> tasksToDelete;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("[TNNUtils.Tween] You can only have one Tween MonoBehaviour!");
                return;
            }
                    
            Instance = this;

            DontDestroyOnLoad(this);

            tasks = new List<Task>();
            tasksToDelete = new List<Task>();
        }

        private void FixedUpdate()
        {
            foreach (var task in tasksToDelete)
            {
                tasks.Remove(task);
            }
            tasksToDelete.Clear();

            foreach (var task in tasks.ToArray())
            {
                task.time += Time.fixedDeltaTime;
                task.normalizedProcess = task.time / task.goalTime;

                if (task.normalizedProcess < 1.0f)
                {
                    UpdateTask(task);
                }
                else
                {
                    task.normalizedProcess = 1f;
                    UpdateTask(task);
                    CompleteTask(task);
                }
            }
        }

        public static void CompleteTask(Task task)
        {
            task.Callback?.Invoke();
            DeleteTask(task);
        }

        public static void DeleteTask(Task task)
        {
            Instance.tasksToDelete.Add(task);
        }

        public enum TaskType
        {
            Move,
            MoveLocal,
            Rotate,
            RotateLocal,
            ScaleLocal,
            MaterialColor,
            ImageTintColor
        };

        [Serializable]
        public class Task
        {
            public float normalizedProcess;
            public float goalTime;
            public float time;
            public Easing.Ease easing;

            public TaskType type;
            public Hashtable Data;

            public Action Callback;
        }

        private static Task AddTask(float time, Easing.Ease easing)
        {
            return new()
            {
                normalizedProcess = 0f,
                time = 0f,
                goalTime = time,
                easing = easing,
                Data = new Hashtable()
            };
        }

        private static void UpdateTask(Task task)
        {
            switch (task.type)
            {
                //TODO: Look for a way to reduce the heavy null comparison. Also look for a way to reduce the amount of repetition.
                case TaskType.Move:
                    if ((Transform)task.Data["transform"] == null) { Debug.LogWarning("[TNNUtils.Tween] Can't update task without data"); DeleteTask(task); break; }
                    ((Transform)task.Data["transform"]).position = LerpVector3WithEase(
                    (Vector3)task.Data["from"],
                    (Vector3)task.Data["to"],
                    task.normalizedProcess,
                    task.easing);
                    break;
                case TaskType.MoveLocal:
                    if ((Transform)task.Data["transform"] == null) { Debug.LogWarning("[TNNUtils.Tween] Can't update task without data"); DeleteTask(task); break; }
                    ((Transform)task.Data["transform"]).localPosition = LerpVector3WithEase(
                    (Vector3)task.Data["from"],
                    (Vector3)task.Data["to"],
                    task.normalizedProcess,
                    task.easing);
                    break;
                case TaskType.Rotate:
                    if ((Transform)task.Data["transform"] == null) { Debug.LogWarning("[TNNUtils.Tween] Can't update task without data"); DeleteTask(task); break; }
                    ((Transform)task.Data["transform"]).eulerAngles = LerpVector3WithEase(
                    (Vector3)task.Data["from"],
                    (Vector3)task.Data["to"],
                    task.normalizedProcess,
                    task.easing);
                    break;
                case TaskType.RotateLocal:
                    if ((Transform)task.Data["transform"] == null) { Debug.LogWarning("[TNNUtils.Tween] Can't update task without data"); DeleteTask(task); break; }
                    ((Transform)task.Data["transform"]).localEulerAngles = LerpVector3WithEase(
                    (Vector3)task.Data["from"],
                    (Vector3)task.Data["to"],
                    task.normalizedProcess,
                    task.easing);
                    break;
                case TaskType.ScaleLocal:
                    if ((Transform)task.Data["transform"] == null) { Debug.LogWarning("[TNNUtils.Tween] Can't update task without data"); DeleteTask(task); break; }
                    ((Transform)task.Data["transform"]).localScale = LerpVector3WithEase(
                    (Vector3)task.Data["from"],
                    (Vector3)task.Data["to"],
                    task.normalizedProcess,
                    task.easing);
                    break;
                case TaskType.MaterialColor:
                    if ((Material)task.Data["material"] == null) { Debug.LogWarning("[TNNUtils.Tween] Can't update task without data"); DeleteTask(task); break; }
                    ((Material)task.Data["material"]).color = LerpColorWithEase(
                    (Color)task.Data["from"],
                    (Color)task.Data["to"],
                    task.normalizedProcess,
                    task.easing);
                    break;
                case TaskType.ImageTintColor:
                    if ((Image)task.Data["image"] == null) { Debug.LogWarning("[TNNUtils.Tween] Can't update task without data"); DeleteTask(task); break; }
                    ((Image)task.Data["image"]).tintColor = LerpColorWithEase(
                    (Color)task.Data["from"],
                    (Color)task.Data["to"],
                    task.normalizedProcess,
                    task.easing);
                    break;
                default:
                    Debug.LogWarning($"[TNNUtils.Tween] UpdateTask not implemented for TaskType: {task.type}");
                    break;
            }
        }

        private static Vector3 LerpVector3WithEase(Vector3 posFrom, Vector3 posTo, float t, Easing.Ease easing)
        {
            return Vector3.Lerp(posFrom, posTo, Easing.EaseResult(easing, 0f, 1f, t));
        }

        private static Color LerpColorWithEase(Color colFrom, Color colTo, float t, Easing.Ease easing)
        {
            return new(
                Mathf.Lerp(colFrom.r, colTo.r, Easing.EaseResult(easing, 0f, 1f, t)),
                Mathf.Lerp(colFrom.g, colTo.g, Easing.EaseResult(easing, 0f, 1f, t)),
                Mathf.Lerp(colFrom.b, colTo.b, Easing.EaseResult(easing, 0f, 1f, t)),
                Mathf.Lerp(colFrom.a, colTo.a, Easing.EaseResult(easing, 0f, 1f, t))
            );
        }

        public static Task MoveTo(Transform obj, Vector3 posTo, float time, Easing.Ease easing, Action callback = null)
        {
            var task = AddTask(time, easing);
            task.type = TaskType.Move;
            task.Data["transform"] = obj;
            task.Data["from"] = obj.position;
            task.Data["to"] = posTo;
            task.Callback = callback;
            Instance.tasks.Add(task);
            return task;
        }

        public static Task MoveToLocal(Transform obj, Vector3 posTo, float time, Easing.Ease easing, Action callback = null)
        {
            var task = AddTask(time, easing);
            task.type = TaskType.MoveLocal;
            task.Data["transform"] = obj;
            task.Data["from"] = obj.localPosition;
            task.Data["to"] = posTo;
            task.Callback = callback;
            Instance.tasks.Add(task);
            return task;
        }

        public static Task Rotate(Transform obj, Vector3 rotTo, float time, Easing.Ease easing, Action callback = null)
        {
            var task = AddTask(time, easing);
            task.type = TaskType.Rotate;
            task.Data["transform"] = obj;
            task.Data["from"] = obj.eulerAngles;
            task.Data["to"] = rotTo;
            task.Callback = callback;
            Instance.tasks.Add(task);
            return task;
        }

        public static Task RotateLocal(Transform obj, Vector3 rotTo, float time, Easing.Ease easing, Action callback = null)
        {
            var task = AddTask(time, easing);
            task.type = TaskType.RotateLocal;
            task.Data["transform"] = obj;
            task.Data["from"] = obj.localEulerAngles;
            task.Data["to"] = rotTo;
            task.Callback = callback;
            Instance.tasks.Add(task);
            return task;
        }

        public static Task ScaleLocal(Transform obj, Vector3 rotTo, float time, Easing.Ease easing, Action callback = null)
        {
            var task = AddTask(time, easing);
            task.type = TaskType.ScaleLocal;
            task.Data["transform"] = obj;
            task.Data["from"] = obj.localScale;
            task.Data["to"] = rotTo;
            task.Callback = callback;
            Instance.tasks.Add(task);
            return task;
        }

        public static Task MaterialColor(Material mat, Color colorTo, float time, Easing.Ease easing, Action callback = null)
        {
            var task = AddTask(time, easing);
            task.type = TaskType.MaterialColor;
            task.Data["material"] = mat;
            task.Data["from"] = mat.color;
            task.Data["to"] = colorTo;
            task.Callback = callback;
            Instance.tasks.Add(task);
            return task;
        }

        public static Task ImageTintColor(Image image, Color colorTo, float time, Easing.Ease easing, Action callback = null)
        {
            var task = AddTask(time, easing);
            task.type = TaskType.ImageTintColor;
            task.Data["image"] = image;
            task.Data["from"] = image.tintColor;
            task.Data["to"] = colorTo;
            task.Callback = callback;
            Instance.tasks.Add(task);
            return task;
        }
    }
}