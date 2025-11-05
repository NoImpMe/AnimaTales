using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class SpawnStage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform nodesCanvas;
    [SerializeField] StageNode firstStagePrefab;
    [SerializeField] StageNode middleStagePrefab;
    [SerializeField] StageNode lastStagePrefab;
    [SerializeField] LineGenerate linePrefab;
    [SerializeField] SpawnLine spawnLine;

    [Header("Grid Settings")]
    [SerializeField] int sectionCount = 10;
    [SerializeField] int sectionPerStage = 3;
    [SerializeField] int xGap = 0;
    [SerializeField] int yGap = 0;

    private Rect rect;
    public StageNode startNode;
    private float x_section, y_section, x_section2, y_section2, randX, randY;
    public Color gizmoColor = Color.cyan;
    public float gizmoWidth = 2f;
    public List<List<StageNode>> sections;

    void Start()
    {
        rect = nodesCanvas.rect;
        x_section = rect.width / 2f;
        y_section = rect.height / 2f;
        x_section2 = -(rect.width / 2f);
        y_section2 = -(rect.height / 2f);
        x_section = x_section / sectionCount;
        y_section = y_section / sectionPerStage;
        x_section2 = x_section2 / sectionCount;
        y_section2 = y_section2 / sectionPerStage;
        sections = new List<List<StageNode>>(sectionCount - 1);
        for (int i = 0; i < sectionCount; i++)
            sections.Add(new List<StageNode>());
        firstStageNodeGenerate();
        firstConnectNodes();
        generateStageNodes();
        lastStageNodeGenerate();
        firstDestroyNotConnect();
        connectStageNodes();
        anotherDestroyNotConnect();
        setType();
        spawnLine.draw_lines();
    }

    void firstStageNodeGenerate()
    {
        var stage = Instantiate(firstStagePrefab, nodesCanvas);
        stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section * (sectionCount - 1), 0);
        startNode = stage;
        startNode.stat = "opened";
    }

    void generateStageNodes()
    {
        int idx = 1;
        for (int i = sectionCount - 5; i >= 0; i -= 2)
        {
            for (int j = 0; j < sectionPerStage; j++)
            {
                var stage = Instantiate(middleStagePrefab, nodesCanvas);
                if (j == 0)
                {
                    randX = Random.Range(-xGap, xGap);
                    randY = Random.Range(-yGap, yGap);
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section * i + randX, y_section * j + randY);
                    sections[idx].Add(stage);
                    continue;
                }
                randX = Random.Range(-xGap, xGap);
                randY = Random.Range(-yGap, yGap);
                stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section * i + randX, y_section * j + randY);
                sections[idx].Add(stage);

                randX = Random.Range(-xGap, xGap);
                randY = Random.Range(-yGap, yGap);
                stage = Instantiate(middleStagePrefab, nodesCanvas);
                stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section * i + randX, y_section2 * j + randY);
                sections[idx].Add(stage);
            }
            idx++;
        }

        for (int i = 1; i < sectionCount - 1; i += 2)
        {
            for (int j = 0; j < sectionPerStage; j++)
            {
                var stage = Instantiate(middleStagePrefab, nodesCanvas);
                if (j == 0)
                {
                    randX = Random.Range(-xGap, xGap);
                    randY = Random.Range(-yGap, yGap);
                    stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section2 * i + randX, y_section * j + randY);
                    sections[idx].Add(stage);
                    continue;
                }
                randX = Random.Range(-xGap, xGap);
                randY = Random.Range(-yGap, yGap);
                stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section2 * i + randX, y_section * j + randY);
                sections[idx].Add(stage);

                randX = Random.Range(-xGap, xGap);
                randY = Random.Range(-yGap, yGap);
                stage = Instantiate(middleStagePrefab, nodesCanvas);
                stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section2 * i + randX, y_section2 * j + randY);
                sections[idx].Add(stage);
            }
            idx++;
        }

        for (int i = 1; i < sections.Count - 1; i++)
        {
            sections[i].Sort((a, b) =>
            b.GetComponent<RectTransform>().anchoredPosition.y
             .CompareTo(
            a.GetComponent<RectTransform>().anchoredPosition.y)
            );
            for (int j = 0; j < sections[i].Count; j++)
            {
                sections[i][j].idx = j;
            }
        }
    }

    void lastStageNodeGenerate()
    {
        var stage = Instantiate(lastStagePrefab, nodesCanvas);

        stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section2 * 8 + 300, y_section * 0);
        sections[8].Add(stage);
        sections[8][0].isBoss = true;
    }

    void connectStageNodes()
    {
        for (int i = 0; i < sections.Count - 3; ++i)
        {
            for (int j = 0; j < sections[i].Count; j++) 
            {
                repeatPrevCountConnect(sections[i][j], i);
            }
        }
        
        for (int i = 0; i < sections[7].Count; i++)
        {
            if (sections[7][i].isSelected)
            {
                sections[7][i].nextNodes.Add(sections[8][0]);
                sections[8][0].prevNodes.Add(sections[7][i]);
            }
            sections[8][0].isSelected = true;
        }
    }

    private bool HasCrossing(StageNode from, StageNode to)
    {
        Vector2 p = from.transform.position;
        Vector2 q = to.transform.position;

        for (int s = 0; s < sections.Count; s++)
        {
            foreach (var node in sections[s])
            {
                foreach (var prev in node.prevNodes)
                {
                    if (prev == from && node == to)
                        continue;

                    Vector2 a = prev.transform.position;
                    Vector2 b = node.transform.position;

                    if (SegmentsIntersect(p, q, a, b))
                        return true;
                }
            }
        }
        return false;
    }
    private bool SegmentsIntersect(Vector2 p, Vector2 q, Vector2 r, Vector2 s)
    {
        float CCW(Vector2 u, Vector2 v, Vector2 w)
            => (v.x - u.x) * (w.y - u.y) - (v.y - u.y) * (w.x - u.x);

        return CCW(p, q, r) * CCW(p, q, s) < 0
            && CCW(r, s, p) * CCW(r, s, q) < 0;
    }

    void repeatPrevCountConnect(StageNode stage, int sectionNum)
    {
        var nextSection = sections[sectionNum + 1];
        var offsets = new List<int> {-2, -1, 0, 1, 2 };

        for (int k = 0; k < stage.prevNodeCount; k++)
        {
            Shuffle(offsets);

            bool connected = false;
            foreach (int off in offsets)
            {
                int targetRow = stage.idx + off;
                if (targetRow < 0 || targetRow >= nextSection.Count)
                    continue;

                var candidate = nextSection[targetRow];

                if (!HasCrossing(stage, candidate))
                {
                    stage.nextNodes.Add(candidate);
                    candidate.prevNodes.Add(stage);
                    candidate.isSelected = true;
                    candidate.prevNodeCount += 1;
                    connected = true;
                    break;
                }
            }
        }
    }
    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }


    void firstConnectNodes()
    {
        var stage = Instantiate(middleStagePrefab, nodesCanvas);

        stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section * (sectionCount - 3), y_section * 2);
        startNode.nextNodes.Add(stage);
        stage.isSelected = true;
        sections[0].Add(stage);

        stage = Instantiate(middleStagePrefab, nodesCanvas);
        stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section * (sectionCount - 3), y_section * 0);
        startNode.nextNodes.Add(stage);
        stage.isSelected = true;
        sections[0].Add(stage);

        stage = Instantiate(middleStagePrefab, nodesCanvas);
        stage.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_section * (sectionCount - 3), y_section2 * 2);
        startNode.nextNodes.Add(stage);
        stage.isSelected = true;
        sections[0].Add(stage);

        for (int i = 0; i < sections[0].Count; i++)
        {
            sections[0][i].idx = i;
            sections[0][i].prevNodeCount += 1;
        }
    }

    void firstDestroyNotConnect()
    {
        sections[0].RemoveAll(n => !n.isSelected);
    }

    void anotherDestroyNotConnect()
    {
        for (int i = 0; i < sections.Count; i++)
        {
            for (int j = 0; j < sections[i].Count; j++)
            {
                if (!sections[i][j].isSelected)
                {
                    Destroy(sections[i][j].gameObject);
                }
            }
            sections[i].RemoveAll(n => !n.isSelected);
        }
    }

    void setType()
    {
        List<int> types = new List<int> { 0, 1, 2, 3, 4, 5 };
        int selectType;
        selectType = Random.Range(0, 6);
        startNode.type = selectType;
        selectType = Random.Range(0, 6);
        sections[8][0].type = selectType;
        for (int i = 0; i < sections.Count; i++)
        {
            for (int j = 0; j < sections[i].Count; j++)
            {
                selectType = Random.Range(0, types.Count);
                sections[i][j].type = selectType;
                sections[i][j].stat = "locked";
            }
        }

       
    }

    public void find_section(StageNode node)
    {
        int x, y;
        if (node == startNode || sections[8][0] == node)
        {
            return;
        }

        for (int i = 0; i < sections.Count; i++)
        {
            for (int j = 0;j < sections[i].Count; j++)
            {
                if (sections[i][j] == node)
                {
                    x = i;
                    y = j;
                    disabled_node(x, y);
                    return;
                }
            }
        }
    }

    void disabled_node(int x, int y)
    {
        for (int i = 0; i < sections[x].Count; i++)
        {
            if (i == y)
            {
                continue;
            }
            else
            {
                sections[x][i].stat = "locked";
                for (int j = 0; j < sections[x][i].lines.Count; j++)
                {
                    sections[x][i].lines[j].setInvalidColor();
                }
            }
        }
        
        if (x == 0)
        {
            for (int i = 0; i < startNode.lines.Count; i++)
            {
                if (i == y)
                {
                    continue;
                }
                startNode.lines[i].setInvalidColor();
            }
        }

        for (int i = 0; i < sections[x].Count; i++)
        {
            Debug.Log($"{x} Sections {i} 번째 스테이지 상태 : {sections[x][i].stat}");
        }
        for(int i = 0; i < sections.Count; i++)
        {
            Debug.Log($"{i} Sections 개수 : {sections[i].Count}");
        }
    } 
}