using CommonLib;
using CommonLib.Animations;
using System.Windows.Controls;

public class SheepAnimation : AnimationTemplate<IAnimationAction>
{
    public SheepAnimation(TextBox output) : base(output)
    {


        _frames = GetFrames();
    }



    protected override List<string> GetFrames() => new List<string>
        {
@"
  __  __       ┌────────────────────┐
 (oo)\___      │                    │  
 (__)\    )\   │All Woman Are sheep!│
     ||--||  　│    　　 　　    　  │　　　
　　　         ⎿＿＿＿＿＿＿＿＿＿＿＿⏌
",
@"
  __  __       ┌────────────────────┐
 (oo)\___      │                    │  
 (__)\    )\   │All Woman Are sheep!│
     ／＼--／| │    　　 　　    　  │　　　
　　　         ⎿＿＿＿＿＿＿＿＿＿＿＿⏌
"


        };
}



