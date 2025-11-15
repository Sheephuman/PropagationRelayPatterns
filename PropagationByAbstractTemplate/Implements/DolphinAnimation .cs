namespace PropagationByAbstractTemplate.Implements
{
    using global::CommonLib;
    using global::CommonLib.Animations;
    using System.Collections.Generic;
    using System.Windows.Controls;

    namespace CommonLib.Animations
    {


        public class DolphinAnimation : AnimationTemplate<IAnimationAction>
        {
            public DolphinAnimation(TextBox output) : base(output)
            {


                _frames = GetFrames();
            }

            protected override List<string> GetFrames() => new List<string>
    {
        @"　　　　　　　
                    -─-‘- 、i
　　　     ＿_, ‘´　　　　　　　ヽ、
　　　    ’,ー–　●　　　　　　　ヽ、
　　　     ｀""’ゝ、_　　　　　　　　　 ‘,
　　　　　　    〈｀’ｰ;＝=  ヽ、〈ｰ- 、 !! /  
　　　　　　     ｀ｰ´　　　    　ヽi   ｀ヽ iﾉ         　　 
    " ,

        { @"    
                    -─-‘- 、i        
　　　     ＿_, ‘´　　　　　　　ヽ、         ´_‘´        _  
　　　    ’,ー–　●　　　　　　　ヽ、! ,,,,     7
　　　     ｀""’ゝ、_　　　　　　　　　     ‘,
　　　　　　    〈｀’ｰ;＝=ヽ、〈ｰ- 、         /
　　　　　　     ｀ｰ´　　　　ヽi｀ヽ ,,,・ 
　　　　　　    "
    }







        };
        }
    }
}


