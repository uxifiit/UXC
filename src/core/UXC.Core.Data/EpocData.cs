/////////////////////////////////////////////////////////////
//                                                         //
//             (c) Softec, spol. s r. o., 2015             //
//                                                         //
/////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UxLabClass.Adapters.Entities.Data;
using UxLabClass.Common;


namespace UxLabClass.Adapters.Entities.Data
{
    [Serializable]
    public class EpocDataAffective : RecordingData
    {
        //affective
     //   public float timeFromStart { get; set; }
        public float longTermExcitementScore { get; set; }
        public float shortTermExcitementScore { get; set; }
        public float meditationScore { get; set; }
        public float frustrationScore { get; set; }
        public float boredomScore { get; set; }
        public float valenceScore { get; set; }
    }

    [Serializable]
    public class EpocDataExpresive : RecordingData
    {
        //expresive
        //public float timeFromStart { get; set; }
        public float eyebrowExtent { get; set; }
        public float smileExtent { get; set; }
        public EE_ExpressivAlgo_t upperFaceAction { get; set; }
        public float upperFacePower { get; set; }
        public double clenchExtent { get; set; }
        public EE_ExpressivAlgo_t lowerFaceAction { get; set; }
        public float lowerFacePower { get; set; }
    }

    [Serializable]
    public class EpocDataCognitive : RecordingData
    {
        //cognitive
       // public float timeFromStart { get; set; }
        public EE_CognitivAction_t cogAction { get; set; }
        public float power { get; set; }
        public bool isActive { get; set; }
    }

    public enum EE_ExpressivAlgo_t
    {
        EXP_NEUTRAL = 0x0001,
        EXP_BLINK = 0x0002,
        EXP_WINK_LEFT = 0x0004,
        EXP_WINK_RIGHT = 0x0008,
        EXP_HORIEYE = 0x0010,
        EXP_EYEBROW = 0x0020,
        EXP_FURROW = 0x0040,
        EXP_SMILE = 0x0080,
        EXP_CLENCH = 0x0100,
        EXP_LAUGH = 0x0200,
        EXP_SMIRK_LEFT = 0x0400,
        EXP_SMIRK_RIGHT = 0x0800
    } 

    public enum EE_CognitivAction_t
    {
        COG_NEUTRAL = 0x0001,
        COG_PUSH = 0x0002,
        COG_PULL = 0x0004,
        COG_LIFT = 0x0008,
        COG_DROP = 0x0010,
        COG_LEFT = 0x0020,
        COG_RIGHT = 0x0040,
        COG_ROTATE_LEFT = 0x0080,
        COG_ROTATE_RIGHT = 0x0100,
        COG_ROTATE_CLOCKWISE = 0x0200,
        COG_ROTATE_COUNTER_CLOCKWISE = 0x0400,
        COG_ROTATE_FORWARDS = 0x0800,
        COG_ROTATE_REVERSE = 0x1000,
        COG_DISAPPEAR = 0x2000

    } 
}
