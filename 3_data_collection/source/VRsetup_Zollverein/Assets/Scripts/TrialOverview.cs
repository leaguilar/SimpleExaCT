using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class TrialOverview : SetupPage
    {
    	public GameObject variation1;
    	
    	public GameObject variation2;
    
        public int Repetitions = 3;
        
        public Text HeaderText;

        public Text DescriptionText;

        public Image DescriptionImage;

        public TMP_Text HintText;

        public Image HintImage;

        public FirstPersonController FPSController;

        public ParticipantRecorder Recorder;

        public GameObject Spawnpoint;

        public string NextSceneName;

        private List<Target> tasks;
        
        private List<Material> materials;

        private int totalTasks;

        private Target currentTarget;

        private Material currentMaterial;
        
        private Material[] NonTargetDoorMaterials;
        private Door[] AllDoors;

        private void Awake()
        {
            // Always same trial for same participant id
            Random.InitState(Database.ParticipantId?.GetHashCode() / 2  ?? 0 +
                             SceneManager.GetActiveScene().name.GetHashCode() / 2);
                                           
            NonTargetDoorMaterials = Resources.LoadAll<Material>("NonTargetDoorMaterials");

            
            var allTargets = FindObjectsOfType<Target>();
            AllDoors = FindObjectsOfType<Door>();
            
            tasks = new List<Target>(allTargets.Length * Repetitions);
            for (var i = 0; i < Repetitions; i++)
            {
                RandomizeOrder(allTargets);
                tasks.AddRange(allTargets);
            }
            
            
            
            Debug.Log(Repetitions);
            Debug.Log(allTargets.Length);

            this.totalTasks = this.tasks.Count;
            
            var allMaterials = Resources.LoadAll<Material>("TargetDoorMaterials/");
            materials = new List<Material>(allTargets.Length * Repetitions);

            if (allMaterials.Length == 0)
            {
                Debug.LogError("No target materials available.");
                this.enabled = false;
                Application.Quit(666);
                return;
            }
            
            Debug.Log(Database.ParticipantGroup);
            if (Database.ParticipantGroup == 2){
            	variation2.SetActive(true);
            }else{
            	variation1.SetActive(true);
            }
            

            while (materials.Count < tasks.Count)
            {
                RandomizeOrder(allMaterials);

                foreach (var material in allMaterials)
                {
                    materials.Add(material);

                    if (materials.Count >= tasks.Count)
                    {
                        break;
                    }
                }
            }
        }

        protected new void OnEnable()
        {
            base.OnEnable();

            Database.EndTrial();

            if (tasks.Count == 0)
            {
                SceneManager.LoadScene(NextSceneName, LoadSceneMode.Single);
                return;
            }

            FPSController.enabled = false;
            Recorder.StopRecording();
            
            foreach (var door in AllDoors)
            {
                var random = Random.Range(0, NonTargetDoorMaterials.Length);
                var randomMaterial = NonTargetDoorMaterials[random];
                var doorRenderer = door.gameObject.GetComponent<Renderer>();
                var mats = doorRenderer.sharedMaterials;
                mats[0] = randomMaterial;
                doorRenderer.sharedMaterials = mats;
            }
            
    
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            currentTarget = tasks[0];
            tasks.RemoveAt(0);

            currentMaterial = materials[0];
            materials.RemoveAt(0);
            var materialName = currentMaterial.name.ToLowerInvariant();

            var renderer = currentTarget.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogError("The target has no renderer attached.");
                this.enabled = false;
                Application.Quit(666);
                return;
            }

            renderer.sharedMaterial = currentMaterial;

            HeaderText.text = $"Task Goal #{Database.TrialResults.Count + 1} of {this.totalTasks}";
            DescriptionText.text = $"Go go to through door with the frame of the following color.";
            DescriptionImage.color = currentMaterial.color;
        }

        protected override void OnApplyPage()
        {
            Debug.Log("Applying page");
            if (HintText != null)
            {
                HintText.text = $"Frame color: {currentMaterial.name.ToLowerInvariant()}";
            }

            if (HintImage != null)
            {
                HintImage.color = currentMaterial.color;
            }

            PlaceFPSController();
            FPSController.enabled = true;
            Recorder.StartRecording();

            if (currentTarget != null)
            {
                Database.StartNewTrial(currentTarget.Number, string.Empty);
            }
        }

        protected override bool CanApplyPage()
        {
            return true;
        }

        private void PlaceFPSController()
        {
            var position = Spawnpoint.transform.position + Vector3.up * 0.9f;
            var rotation = Quaternion.Euler(0, -90, 0);

            FPSController.gameObject.PlaceObject(position, rotation);
        }

        private void RandomizeOrder<T>(IList<T> items)
        {
            var cnt = items.Count;
            for (var i = 0; i < cnt; i++)
            {
                var newIndex = Random.Range(0, cnt);
                var tmp = items[i];
                items[i] = items[newIndex];
                items[newIndex] = tmp;
            }
        }
    }
}
