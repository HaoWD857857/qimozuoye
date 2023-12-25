using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class skillManager1 : MonoBehaviour
{
    public List<ISkill> skills = new List<ISkill>(); // 存放所有技能
    // 在 Unity Inspector 视图中手动将技能添加到 skills 列表中
    public FireSkill01 fireSkill; // 在 Inspector 视图中将 FireSkill 实例拖拽到这个变量上
    public FireSkill02 skill_W;  //动画技能
    public Skill03 skill_R;  //动画技能
    public Skill_E skill_E; //动画技能
    //public ... E技能回血，等做了血条在做，动画效果用哪个胜利

    private Animator ani;//配合技能动画
    private float QTimeless = 0;
    void Start()
    {
        ani = GetComponent<Animator>();

        // 将手动添加的技能加入到技能列表中
        if (fireSkill != null)
        {
            skills.Add(fireSkill);
        }
        if (skill_W != null)
        {
            skills.Add(skill_W);
        }
        if (skill_R != null)
        {
            skills.Add(skill_R);
        }
        if (skill_E != null)
        {
            skills.Add(skill_E);
        }
    }

    void Update()
    {
        QTimeless -= Time.deltaTime;
        //获取鼠标射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //碰撞信息
        RaycastHit hit;
        //检测碰撞
        if (Physics.Raycast(ray, out hit))
        {
            //角色看向碰撞方向
            //transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));

            //走路的时候放技能player就会飘移
            if(PlayerCol.IsMoving == false)
            {
                
                if (Input.GetKeyDown(KeyCode.W))
                {
                    ActivateSkill(1);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ActivateSkill(2);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q) && QTimeless<=0)
            {
                ani.GetComponent<NavMeshAgent>().isStopped = true;
                //先放动画，再实例火球
                ani.SetTrigger("SkillTriggerQ");
                //因为先放动画，再实例火球，所以两边CD不同步
                QTimeless = 3.5f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateSkill(3);
            }

        }
    }
    //放到动画事件里面去了
    void ActiveteSkill_Q()
    {
        ActivateSkill(0);
    }
    public void ActivateSkill(int index)
    {
        if (index >= 0 && index < skills.Count)
        {
            skills[index].Activate(); // 激活对应索引的技能
        }
    }
}
