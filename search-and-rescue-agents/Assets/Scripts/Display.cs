 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
 using System.Timers;

 public class Display : MonoBehaviour {

     Text txt;
     public static int currentRescued = 0;
     public static int agentCount = 0;
     public static int humans = 0;
     private static int time = 0;

     Time timer;

     // Use this for initialization
     void Start () {
         txt = gameObject.GetComponent<Text>();
         txt.text = "Rescued humans : " + currentRescued;
         txt.text += "\nTotal agents : " + agentCount;
         txt.text += "\nTotal humans : " + humans;
     }

     // Update is called once per frame
     void Update () {

         txt.text = "Time : " + time + " seconds";
         txt.text += "\nRescued humans : " + currentRescued;
         txt.text += "\nTotal humans : " + humans;
         txt.text += "\nTotal agents : " + agentCount;

         if (time == 0 && agentCount > 0) {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed+=new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval=1000;
            aTimer.Enabled=true;
            time++;
         }

     }

     private static void OnTimedEvent(object source, ElapsedEventArgs e)
      {
          if (currentRescued != humans)
            time++;
      }

 }