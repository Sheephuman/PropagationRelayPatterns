using System.Windows.Controls;


namespace CommonLib.Animations
{

    public class BirdAnimation : AnimationTemplate<IAnimationAction>
    {

        public BirdAnimation(TextBox output) : base(output)
        {
            _frames = GetFrames();
        }

        protected override List<string> GetFrames() => new List<string>
        {

@"＼```＼        ＼ ``＼ 
 ,,,＼, ＼        ＼ ``  ＼ ,
＿''"",e,""゛ヽ ＼ ､､＼ 
￣ヽ,　｀ヽ: : : : : : : : : :ﾞヽ
     ヽ, ヽ､､_,.,.,.,.-''"" 
      ヽ､､､___,,,,.,.-''"" 
  

",
@"
      ,,,,,
        ＿'"",e,゛ヽ､､､､､　　　＼ 
        ￣ヽ,　｀ヽ: : : : :ﾞヽ＼ ＼
             ヽ,　　ヽ､､_,.,.-'"" 　＼ 
              ヽ､､､___,,.-'"" ＼  ``   ``＼ 
                ＼   ／           ＼＼    ＼
                 ＼ ／                ＼＼
                   ＼↑／"
        };






    }
}