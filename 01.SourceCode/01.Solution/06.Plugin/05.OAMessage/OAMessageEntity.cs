using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Plugin.OAMessage
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateAttachment.
    /// </summary>
    [ORTableMapping("dbo.OAMessage")]
    public class OAMessageEntity : BaseModel
    {
        /// <summary>
        /// ���ؽ��
        /// </summary>
        [ORFieldMapping("ResultData")]
        public string ResultData { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        [ORFieldMapping("FlowID")]
        public string FlowID { get; set; }
        /// <summary>
        /// ���̱���
        /// </summary>
        [ORFieldMapping("FlowTitle")]
        public string FlowTitle { get; set; }
        /// <summary>
        /// ʹ�ù���������
        /// </summary>
        [ORFieldMapping("WorkflowName")]
        public string WorkflowName { get; set; }
        /// <summary>
        /// ��ǰ�û��ڵ�����
        /// </summary>
        [ORFieldMapping("NodeName")]
        public string NodeName { get; set; }
        /// <summary>
        /// ���Զ�������ַ
        /// </summary>
        [ORFieldMapping("PCUrl")]
        public string PCUrl { get; set; }
        /// <summary>
        /// �ֻ���������ַ
        /// </summary>
        [ORFieldMapping("AppUrl")]
        public string AppUrl { get; set; }
        /// <summary>
        /// ���������û�
        /// </summary>
        [ORFieldMapping("CreateFlowUser")]
        public string CreateFlowUser { get; set; }
        /// <summary>
        /// ��������ʱ��
        /// </summary>
        [ORFieldMapping("CreateFlowTime")]
        public DateTime CreateFlowTime { get; set; }
        /// <summary>
        /// ���������û�
        /// </summary>
        [ORFieldMapping("ReceiverFlowUser")]
        public string ReceiverFlowUser { get; set; }
        /// <summary>
        /// ��������ʱ��
        /// </summary>
        [ORFieldMapping("ReceiverFlowTime")]
        public DateTime ReceiverFlowTime { get; set; }
        /// <summary>
        /// ���̴���״̬ 0���� 2�Ѱ� 4���
        /// </summary>
        [ORFieldMapping("FlowType")]
        public int FlowType { get; set; }
        /// <summary>
        /// ���̲鿴״̬ 0δ�� 1�Ѷ�
        /// </summary>
        [ORFieldMapping("ViewType")]
        public int ViewType { get; set; }


    }
}

