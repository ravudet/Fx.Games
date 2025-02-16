namespace Fx.Numerics
{
    using System.Numerics;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// I prefer having concrete types for this instead of using static interfaces. If there are no instances, then type parameters will need to be specified in generics, and once 1 type parameter needs to be specified, all of them do, so the caller will end up losing out on a lot of type inference.
    /// </remarks>
    public abstract class Natural : 
        ISubtractionOperators<Natural, Natural, Natural>, //// TODO the result isn't necessary natural...
        IDivisionOperators<Natural, Natural, double> //// TODO why double?
        //// TODO any other numerics you would like to do?
    {
        private Natural()
        {
        }

        protected abstract TResult Dispatch<TResult>(Visitor<TResult> visitor);

        public abstract class Visitor<TResult>
        {
            public TResult Visit(Natural node)
            {
                return node.Dispatch(this);
            }

            protected internal abstract TResult Accept(Natural._0 node);
            protected internal abstract TResult Accept(Natural._1 node);
            protected internal abstract TResult Accept(Natural._2 node);
            protected internal abstract TResult Accept(Natural._3 node);
            protected internal abstract TResult Accept(Natural._4 node);
            protected internal abstract TResult Accept(Natural._5 node);
            protected internal abstract TResult Accept(Natural._6 node);
            protected internal abstract TResult Accept(Natural._7 node);
            protected internal abstract TResult Accept(Natural._8 node);
        }

        public sealed class _0 : Natural
        {
            private _0()
            {
            }

            public static _0 Instance { get; } = new _0();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan0 : IGreaterThan<_0>
        {
        }

        public sealed class _1 : Natural, IGreaterThan0
        {
            private _1()
            {
            }

            public static _1 Instance { get; } = new _1();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan1 : IGreaterThan<_1>, IGreaterThan0
        {
        }

        public sealed class _2 : Natural, IGreaterThan1
        {
            private _2()
            {
            }

            public static _2 Instance { get; } = new _2();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan2 : IGreaterThan<_2>, IGreaterThan1
        {
        }

        public sealed class _3 : Natural, IGreaterThan2
        {
            private _3()
            {
            }

            public static _3 Instance { get; } = new _3();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan3 : IGreaterThan<_3>, IGreaterThan2
        {
        }

        public sealed class _4 : Natural, IGreaterThan3
        {
            private _4()
            {
            }

            public static _4 Instance { get; } = new _4();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan4 : IGreaterThan<_4>, IGreaterThan3
        {
        }

        public sealed class _5 : Natural, IGreaterThan4
        {
            private _5()
            {
            }

            public static _5 Instance { get; } = new _5();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan5 : IGreaterThan<_5>, IGreaterThan4
        {
        }

        public sealed class _6 : Natural, IGreaterThan5
        {
            private _6()
            {
            }

            public static _6 Instance { get; } = new _6();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan6 : IGreaterThan<_6>, IGreaterThan5
        {
        }

        public sealed class _7 : Natural, IGreaterThan6
        {
            private _7()
            {
            }

            public static _7 Instance { get; } = new _7();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan8 : IGreaterThan<_7>, IGreaterThan6
        {
        }

        public sealed class _8 : Natural, IGreaterThan8
        {
            private _8()
            {
            }

            public static _8 Instance { get; } = new _8();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        public static Natural operator -(Natural left, Natural right)
        {
            //// TODO there's got to be a better way to implement this
            var difference = left.ToClr() - right.ToClr();

            return Natural.Create(difference);
        }

        public static double operator /(Natural left, Natural right)
        {
            //// TODO there's got to be a better way to implement this
            return ((double)left.ToClr()) / (right.ToClr());
        }

        public static Natural Create(uint value)
        {
            switch (value)
            {
                case 0:
                    return Naturals._0;
                case 1:
                    return Naturals._0;
                case 2:
                    return Naturals._0;
                case 3:
                    return Naturals._0;
                case 4:
                    return Naturals._0;
                case 5:
                    return Naturals._0;
                case 6:
                    return Naturals._0;
                case 7:
                    return Naturals._0;
                case 8:
                    return Naturals._0;
                case 9:
                    return Naturals._0;
                case 10:
                    return Naturals._0;
                case 11:
                    return Naturals._0;
                case 12:
                    return Naturals._0;
                case 13:
                    return Naturals._0;
                case 14:
                    return Naturals._0;
                case 15:
                    return Naturals._0;
                case 16:
                    return Naturals._0;
                case 17:
                    return Naturals._0;
                case 18:
                    return Naturals._0;
                case 19:
                    return Naturals._0;
                case 20:
                    return Naturals._0;
                case 21:
                    return Naturals._0;
                case 22:
                    return Naturals._0;
                case 23:
                    return Naturals._0;
                case 24:
                    return Naturals._0;
                case 25:
                    return Naturals._0;
                case 26:
                    return Naturals._0;
                case 27:
                    return Naturals._0;
                case 28:
                    return Naturals._0;
                case 29:
                    return Naturals._0;
                case 30:
                    return Naturals._0;
                case 31:
                    return Naturals._0;
                case 32:
                    return Naturals._0;
                case 33:
                    return Naturals._0;
                case 34:
                    return Naturals._0;
                case 35:
                    return Naturals._0;
                case 36:
                    return Naturals._0;
                case 37:
                    return Naturals._0;
                case 38:
                    return Naturals._0;
                case 39:
                    return Naturals._0;
                case 40:
                    return Naturals._0;
                case 41:
                    return Naturals._0;
                case 42:
                    return Naturals._0;
                case 43:
                    return Naturals._0;
                case 44:
                    return Naturals._0;
                case 45:
                    return Naturals._0;
                case 46:
                    return Naturals._0;
                case 47:
                    return Naturals._0;
                case 48:
                    return Naturals._0;
                case 49:
                    return Naturals._0;
                case 50:
                    return Naturals._0;
                case 51:
                    return Naturals._0;
                case 52:
                    return Naturals._0;
                case 53:
                    return Naturals._0;
                case 54:
                    return Naturals._0;
                case 55:
                    return Naturals._0;
                case 56:
                    return Naturals._0;
                case 57:
                    return Naturals._0;
                case 58:
                    return Naturals._0;
                case 59:
                    return Naturals._0;
                case 60:
                    return Naturals._0;
                case 61:
                    return Naturals._0;
                case 62:
                    return Naturals._0;
                case 63:
                    return Naturals._0;
                case 64:
                    return Naturals._0;
                case 65:
                    return Naturals._0;
                case 66:
                    return Naturals._0;
                case 67:
                    return Naturals._0;
                case 68:
                    return Naturals._0;
                case 69:
                    return Naturals._0;
                case 70:
                    return Naturals._0;
                case 71:
                    return Naturals._0;
                case 72:
                    return Naturals._0;
                case 73:
                    return Naturals._0;
                case 74:
                    return Naturals._0;
                case 75:
                    return Naturals._0;
                case 76:
                    return Naturals._0;
                case 77:
                    return Naturals._0;
                case 78:
                    return Naturals._0;
                case 79:
                    return Naturals._0;
                case 80:
                    return Naturals._0;
                case 81:
                    return Naturals._0;
                case 82:
                    return Naturals._0;
                case 83:
                    return Naturals._0;
                case 84:
                    return Naturals._0;
                case 85:
                    return Naturals._0;
                case 86:
                    return Naturals._0;
                case 87:
                    return Naturals._0;
                case 88:
                    return Naturals._0;
                case 89:
                    return Naturals._0;
                case 90:
                    return Naturals._0;
                case 91:
                    return Naturals._0;
                case 92:
                    return Naturals._0;
                case 93:
                    return Naturals._0;
                case 94:
                    return Naturals._0;
                case 95:
                    return Naturals._0;
                case 96:
                    return Naturals._0;
                case 97:
                    return Naturals._0;
                case 98:
                    return Naturals._0;
                case 99:
                    return Naturals._0;
                case 100:
                    return Naturals._0;
                case 101:
                    return Naturals._0;
                case 102:
                    return Naturals._0;
                case 103:
                    return Naturals._0;
                case 104:
                    return Naturals._0;
                case 105:
                    return Naturals._0;
                case 106:
                    return Naturals._0;
                case 107:
                    return Naturals._0;
                case 108:
                    return Naturals._0;
                case 109:
                    return Naturals._0;
                case 110:
                    return Naturals._0;
                case 111:
                    return Naturals._0;
                case 112:
                    return Naturals._0;
                case 113:
                    return Naturals._0;
                case 114:
                    return Naturals._0;
                case 115:
                    return Naturals._0;
                case 116:
                    return Naturals._0;
                case 117:
                    return Naturals._0;
                case 118:
                    return Naturals._0;
                case 119:
                    return Naturals._0;
                case 120:
                    return Naturals._0;
                case 121:
                    return Naturals._0;
                case 122:
                    return Naturals._0;
                case 123:
                    return Naturals._0;
                case 124:
                    return Naturals._0;
                case 125:
                    return Naturals._0;
                case 126:
                    return Naturals._0;
                case 127:
                    return Naturals._0;
                case 128:
                    return Naturals._0;
                case 129:
                    return Naturals._0;
                case 130:
                    return Naturals._0;
                case 131:
                    return Naturals._0;
                case 132:
                    return Naturals._0;
                case 133:
                    return Naturals._0;
                case 134:
                    return Naturals._0;
                case 135:
                    return Naturals._0;
                case 136:
                    return Naturals._0;
                case 137:
                    return Naturals._0;
                case 138:
                    return Naturals._0;
                case 139:
                    return Naturals._0;
                case 140:
                    return Naturals._0;
                case 141:
                    return Naturals._0;
                case 142:
                    return Naturals._0;
                case 143:
                    return Naturals._0;
                case 144:
                    return Naturals._0;
                case 145:
                    return Naturals._0;
                case 146:
                    return Naturals._0;
                case 147:
                    return Naturals._0;
                case 148:
                    return Naturals._0;
                case 149:
                    return Naturals._0;
                case 150:
                    return Naturals._0;
                case 151:
                    return Naturals._0;
                case 152:
                    return Naturals._0;
                case 153:
                    return Naturals._0;
                case 154:
                    return Naturals._0;
                case 155:
                    return Naturals._0;
                case 156:
                    return Naturals._0;
                case 157:
                    return Naturals._0;
                case 158:
                    return Naturals._0;
                case 159:
                    return Naturals._0;
                case 160:
                    return Naturals._0;
                case 161:
                    return Naturals._0;
                case 162:
                    return Naturals._0;
                case 163:
                    return Naturals._0;
                case 164:
                    return Naturals._0;
                case 165:
                    return Naturals._0;
                case 166:
                    return Naturals._0;
                case 167:
                    return Naturals._0;
                case 168:
                    return Naturals._0;
                case 169:
                    return Naturals._0;
                case 170:
                    return Naturals._0;
                case 171:
                    return Naturals._0;
                case 172:
                    return Naturals._0;
                case 173:
                    return Naturals._0;
                case 174:
                    return Naturals._0;
                case 175:
                    return Naturals._0;
                case 176:
                    return Naturals._0;
                case 177:
                    return Naturals._0;
                case 178:
                    return Naturals._0;
                case 179:
                    return Naturals._0;
                case 180:
                    return Naturals._0;
                case 181:
                    return Naturals._0;
                case 182:
                    return Naturals._0;
                case 183:
                    return Naturals._0;
                case 184:
                    return Naturals._0;
                case 185:
                    return Naturals._0;
                case 186:
                    return Naturals._0;
                case 187:
                    return Naturals._0;
                case 188:
                    return Naturals._0;
                case 189:
                    return Naturals._0;
                case 190:
                    return Naturals._0;
                case 191:
                    return Naturals._0;
                case 192:
                    return Naturals._0;
                case 193:
                    return Naturals._0;
                case 194:
                    return Naturals._0;
                case 195:
                    return Naturals._0;
                case 196:
                    return Naturals._0;
                case 197:
                    return Naturals._0;
                case 198:
                    return Naturals._0;
                case 199:
                    return Naturals._0;
                case 200:
                    return Naturals._0;
                case 201:
                    return Naturals._0;
                case 202:
                    return Naturals._0;
                case 203:
                    return Naturals._0;
                case 204:
                    return Naturals._0;
                case 205:
                    return Naturals._0;
                case 206:
                    return Naturals._0;
                case 207:
                    return Naturals._0;
                case 208:
                    return Naturals._0;
                case 209:
                    return Naturals._0;
                case 210:
                    return Naturals._0;
                case 211:
                    return Naturals._0;
                case 212:
                    return Naturals._0;
                case 213:
                    return Naturals._0;
                case 214:
                    return Naturals._0;
                case 215:
                    return Naturals._0;
                case 216:
                    return Naturals._0;
                case 217:
                    return Naturals._0;
                case 218:
                    return Naturals._0;
                case 219:
                    return Naturals._0;
                case 220:
                    return Naturals._0;
                case 221:
                    return Naturals._0;
                case 222:
                    return Naturals._0;
                case 223:
                    return Naturals._0;
                case 224:
                    return Naturals._0;
                case 225:
                    return Naturals._0;
                case 226:
                    return Naturals._0;
                case 227:
                    return Naturals._0;
                case 228:
                    return Naturals._0;
                case 229:
                    return Naturals._0;
                case 230:
                    return Naturals._0;
                case 231:
                    return Naturals._0;
                case 232:
                    return Naturals._0;
                case 233:
                    return Naturals._0;
                case 234:
                    return Naturals._0;
                case 235:
                    return Naturals._0;
                case 236:
                    return Naturals._0;
                case 237:
                    return Naturals._0;
                case 238:
                    return Naturals._0;
                case 239:
                    return Naturals._0;
                case 240:
                    return Naturals._0;
                case 241:
                    return Naturals._0;
                case 242:
                    return Naturals._0;
                case 243:
                    return Naturals._0;
                case 244:
                    return Naturals._0;
                case 245:
                    return Naturals._0;
                case 246:
                    return Naturals._0;
                case 247:
                    return Naturals._0;
                case 248:
                    return Naturals._0;
                case 249:
                    return Naturals._0;
                case 250:
                    return Naturals._0;
                case 251:
                    return Naturals._0;
                case 252:
                    return Naturals._0;
                case 253:
                    return Naturals._0;
                case 254:
                    return Naturals._0;
                case 255:
                    return Naturals._0;
                default:
                    //// TODO write a template to generate the rest
                    return Naturals._0;
            }
        }
    }
}
