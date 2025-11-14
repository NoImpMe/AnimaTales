//using UnityEditor.Experimental.GraphView;
//using UnityEngine;
//using static UnityEngine.Rendering.CoreUtils;

//public class SpawnLine : MonoBehaviour
//{
//    [SerializeField] SpawnStage spawnStage;
//    [SerializeField] LineGenerate linePrefab;
//    [SerializeField] RectTransform linesCanvas;
//    private StageNode startNode;
//    public void draw_lines()
//    {
//        start_stage_line();
//    }

//    void start_stage_line()
//    {
//        startNode = spawnStage.startNode;
//        for (int i = 0; i < spawnStage.sections[0].Count; i++)
//        {
//            var line = Instantiate(linePrefab, linesCanvas);
//            line.transform.SetParent(linesCanvas, false);
//            line.startPoint = startNode.transform;
//            line.endPoint = spawnStage.sections[0][i].transform;
//            startNode.lines.Add(line);
//        }
//        connect_stage_line();
//    }

//    void connect_stage_line()
//    {
//        for (int i = 0; i < spawnStage.sections.Count; i++)
//        {
//            for (int j = 0; j < spawnStage.sections[i].Count; j++)
//            {
//                for (int k = 0; k < spawnStage.sections[i][j].nextNodes.Count; k++)
//                {
//                    var line = Instantiate(linePrefab, linesCanvas);
//                    line.transform.SetParent(linesCanvas, false);
//                    line.startPoint = spawnStage.sections[i][j].transform;
//                    line.endPoint = spawnStage.sections[i][j].nextNodes[k].transform;
//                    spawnStage.sections[i][j].lines.Add(line);
//                }
//            }
//        }
//    }
//}
