using System;
using System.Xml;
using System.Resources;
using System.Reflection;
using System.Collections;

namespace Framework.Core.Expression
{
	/// <summary>
	/// �������
	/// </summary>
	public enum Parse_Error
	{
		peNone = 0,

		/// <summary>
		/// �Ƿ��ַ�
		/// </summary>
		peInvalidChar,

		/// <summary>
		/// �Ƿ����ַ���
		/// </summary>
		peInvalidString,

		/// <summary>
		/// �Ƿ�������
		/// </summary>
		peInvalidOperator,

		/// <summary>
		/// ���Ͳ�ƥ��
		/// </summary>
		peTypeMismatch,

		/// <summary>
		/// �Ƿ��Ĳ���
		/// </summary>
		peInvalidParam,

		/// <summary>
		/// �Ƿ����û��Զ��庯������ֵ
		/// </summary>
		peInvalidUFValue,

		/// <summary>
		/// �﷨����
		/// </summary>
		peSyntaxError,

		/// <summary>
		/// �����������
		/// </summary>
		peFloatOverflow,

		/// <summary>
		/// ��Ҫĳ���ַ�
		/// </summary>
		peCharExpected,

		/// <summary>
		/// ��������
		/// </summary>
		peFuncError,

		/// <summary>
		/// ��Ҫ������
		/// </summary>
		peNeedOperand,

		/// <summary>
		/// ��ʽ����
		/// </summary>
		peFormatError,
	}

	/// <summary>
	/// Ϊ���ʽʶ������װ���쳣
	/// </summary>
	public class ParsingException: System.Exception
	{
		private Parse_Error _Reason = Parse_Error.peNone;
		private int _Position = -1;

		/// <summary>
		/// ���캯�������ݴ������͡�����λ�ù����쳣
		/// </summary>
		/// <param name="pe"></param>
		/// <param name="nPosition"></param>
		/// <param name="strMsg"></param>
		public ParsingException(Parse_Error pe, int nPosition, string strMsg):base(strMsg)
		{
			_Reason = pe;
			_Position = nPosition;
		}

		/// <summary>
		/// ����ԭ��
		/// </summary>
		public Parse_Error Reason
		{
			get
			{
				return _Reason;
			}
		}

		/// <summary>
		/// ����λ��
		/// </summary>
		public int Position
		{
			get
			{
				return _Position;
			}
		}

		/// <summary>
		/// ����һ���µı��ʽʶ���쳣
		/// </summary>
		/// <param name="pe">����ԭ��</param>
		/// <param name="nPosition">����λ��</param>
		/// <param name="strParams">�ڴ�����Ϣ�еĲ���</param>
		/// <returns>���ʽʶ���쳣����</returns>
		static public ParsingException NewParsingException(Parse_Error pe, int nPosition, params string[] strParams)
		{
            try
            {
                ResourceManager rm = new ResourceManager("Framework.Core.LightWorkflow.Tools", Assembly.GetExecutingAssembly());

                string strID = pe.ToString();

                string strText = rm.GetString(strID);

                strText = string.Format(strText, strParams);

                if (nPosition >= 0)
                {
                    strText = string.Format(rm.GetString("position"), nPosition + 1) + ", " + strText;
                }

                return new ParsingException(pe, nPosition, strText);
            }
            catch
            {
                string msg = string.Empty;
                if (nPosition >= 0)
                {
                    msg = string.Format("�ַ�λ��{0},", nPosition);
                }
                foreach (var item in strParams)
                {
                    msg += "������:";
                    msg += item + ",";
                }

                throw new Exception(string.Format("����һ�����ʽʶ���쳣,{0}", msg.TrimEnd(',')));
            }
		}
	}

	/// <summary>
	/// ��������
	/// </summary>
	public enum Operation_IDs
	{
		OI_NONE = 0,
		OI_NOT = 120,
		OI_ADD,
		OI_MINUS,
		OI_MUL,
		OI_DIV,
		OI_NEG,
		//
		OI_EQUAL,
		OI_NOT_EQUAL,
		OI_GREAT,
		OI_GREATEQUAL,
		OI_LESS,
		OI_LESSEQUAL,
		OI_LOGICAL_AND,
		OI_LOGICAL_OR,
		//
		OI_LBRACKET,
		OI_RBRACKET,
		OI_COMMA,
		//
		OI_USERDEFINE,
		OI_STRING,
		OI_NUMBER,
		OI_BOOLEAN,
		OI_DATETIME,
	}

	public class ParseIdentifier
	{
		internal Operation_IDs _OperationID = Operation_IDs.OI_NONE;
		internal string _Identifier = string.Empty;
		internal int _Position = -1;
		internal ParseIdentifier _PrevIdentifier = null;
		internal ParseIdentifier _NextIdentifier = null;
		internal ParseIdentifier _SubIdentifier = null;
		internal ParseIdentifier _ParentIdentifier = null;

		public Operation_IDs OperationID
		{
			get
			{
				return _OperationID;
			}
		}

		public string Identifier
		{
			get
			{
				return _Identifier;
			}
		}

		public int Position
		{
			get
			{
				return _Position;
			}
		}

		public ParseIdentifier PrevIdentifier
		{
			get
			{
				return _PrevIdentifier;
			}
		}

		public ParseIdentifier NextIdentifier
		{
			get
			{
				return _NextIdentifier;
			}
		}

		public ParseIdentifier SubIdentifier
		{
			get
			{
				return _SubIdentifier;
			}
		}

		public ParseIdentifier ParentIdentifier
		{
			get
			{
				return _ParentIdentifier;
			}
		}

		public ParseIdentifier()
		{
		}

		public ParseIdentifier(Operation_IDs oID, string strID, int nPos, ParseIdentifier prev)
		{
			_OperationID = oID;
			_Identifier = strID;
			_Position = nPos;
			_PrevIdentifier = prev;
		}
	}

	public class ParamObject
	{
		internal object _Value = null;
		internal int _Position = -1;

		public ParamObject()
		{
		}

		public ParamObject(object v, int nPos)
		{
			_Value = v;
			_Position = nPos;
		}

		public object Value
		{
			get
			{
				return _Value;
			}
		}

		public int Position
		{
			get
			{
				return _Position;
			}
		}
	}

	public interface IExpParsing
	{
		object CalculateUserFunction(string strFuncName, ParamObject[] arrParams, ParseExpression parseObj);
		object CheckUserFunction(string strFuncName, ParamObject[] arrParams, ParseExpression parseObj);
	}

	public interface IExpEditor
	{
		XmlNode GetNameSpaceNode();
	}

	/// <summary>
	/// �������Ľڵ�
	/// </summary>
	public class EXP_TreeNode
	{
		internal EXP_TreeNode _Left = null;
		internal EXP_TreeNode _Right = null;
		internal int _Position = 0;
		internal Operation_IDs _OperationID = Operation_IDs.OI_NONE;
		internal Object _Value = null;
		internal ArrayList _Params = null;
		internal string _FunctionName = string.Empty;

		public EXP_TreeNode Left
		{
			get
			{
				return _Left;
			}
		}

		public EXP_TreeNode Right
		{
			get
			{
				return _Right;
			}
		}

		public int Position
		{
			get
			{
				return _Position;
			}
		}

		public Operation_IDs OperationID
		{
			get
			{
				return _OperationID;
			}
		}

		public Object Value
		{
			get
			{
				return _Value;
			}
		}

		public ArrayList Params
		{
			get
			{
				return _Params;
			}
		}

		public string FunctionName
		{
			get
			{
				return _FunctionName;
			}
		}
	}
}
