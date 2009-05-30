/**
 * http://svn.myrealbox.com/viewcvs/trunk/mcs/class/System.Data/System.Data/CustomDataClassGenerator.cs?view=markup
 * //
 * // Mono.Data.CustomDataClassGenerator
 * //
 * // Author:
 * //  Atsushi Enomoto <atsushi@ximian.com>
 * //
 * // (C)2004 Novell Inc.
 * //
 * // API notes are the bottom of the source.
 * //
 * // This class is standalone testable (even under MS.NET) when compiled with
 * // -d:DATACLASS_GENERATOR_STANDALONE .
 * //
 *
 * //
 * // Copyright (C) 2004 Novell, Inc (http://www.novell.com)
 * //
 * // Permission is hereby granted, free of charge, to any person obtaining
 * // a copy of this software and associated documentation files (the
 * // "Software"), to deal in the Software without restriction, including
 * // without limitation the rights to use, copy, modify, merge, publish,
 * // distribute, sublicense, and/or sell copies of the Software, and to
 * // permit persons to whom the Software is furnished to do so, subject to
 * // the following conditions:
 * //
 * // The above copyright notice and this permission notice shall be
 * // included in all copies or substantial portions of the Software.
 * //
 * // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * // MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * // LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * // OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * // WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * //
 */

using System;
using System.CodeDom;
using System.Collections;

namespace Ict.Tools.CodeGeneration
{
    public class CodeDom
    {
        public static CodeThisReferenceExpression This()
        {
            return new CodeThisReferenceExpression();
        }

        public static CodeBaseReferenceExpression Base()
        {
            return new CodeBaseReferenceExpression();
        }

        public static CodePrimitiveExpression _Const(System.Object value)
        {
            return new CodePrimitiveExpression(value);
        }

        public static CodeTypeReference TypeRef(Type t)
        {
            return new CodeTypeReference(t);
        }

        public static CodeTypeReference TypeRef(string name)
        {
            return new CodeTypeReference(name);
        }

        public static CodeParameterDeclarationExpression Param(string t, string name)
        {
            return new CodeParameterDeclarationExpression(t, name);
        }

        public static CodeParameterDeclarationExpression Param(string t, string name, FieldDirection direction)
        {
            CodeParameterDeclarationExpression ReturnValue;

            ReturnValue = new CodeParameterDeclarationExpression(t, name);
            ReturnValue.Direction = direction;
            return ReturnValue;
        }

        public static CodeParameterDeclarationExpression Param(Type t, string name)
        {
            return new CodeParameterDeclarationExpression(t, name);
        }

        public static CodeParameterDeclarationExpression Param(CodeTypeReference t, string name)
        {
            return new CodeParameterDeclarationExpression(t, name);
        }

        public static CodeArgumentReferenceExpression ParamRef(string name)
        {
            return new CodeArgumentReferenceExpression(name);
        }

        public static CodeCastExpression Cast(string t, CodeExpression exp)
        {
            return new CodeCastExpression(t, exp);
        }

        public static CodeCastExpression Cast(Type t, CodeExpression exp)
        {
            return new CodeCastExpression(t, exp);
        }

        public static CodeCastExpression Cast(CodeTypeReference t, CodeExpression exp)
        {
            return new CodeCastExpression(t, exp);
        }

        public static CodeExpression _New(Type t, CodeExpression[] parameters)
        {
            return new CodeObjectCreateExpression(t, parameters);
        }

        public static CodeExpression _New(string t, CodeExpression[] parameters)
        {
            return new CodeObjectCreateExpression(TypeRef(t), parameters);
        }

        public static CodeExpression NewArray(Type t, CodeExpression[] parameters)
        {
            return new CodeArrayCreateExpression(t, parameters);
        }

        public static CodeVariableReferenceExpression Local(string name)
        {
            return new CodeVariableReferenceExpression(name);
        }

        public static CodeFieldReferenceExpression FieldRef(string name)
        {
            return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), name);
        }

        public static CodeFieldReferenceExpression FieldRef(CodeExpression exp, string name)
        {
            return new CodeFieldReferenceExpression(exp, name);
        }

        public static CodePropertyReferenceExpression PropRef(string name)
        {
            return new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), name);
        }

        public static CodePropertyReferenceExpression PropRef(CodeExpression target, string name)
        {
            return new CodePropertyReferenceExpression(target, name);
        }

        public static CodeIndexerExpression IndexerRef(CodeExpression target, CodeExpression[] parameters)
        {
            return new CodeIndexerExpression(target, parameters);
        }

        public static CodeIndexerExpression IndexerRef(CodeExpression[] param)
        {
            return new CodeIndexerExpression(new CodeThisReferenceExpression(), param);
        }

        public static CodeEventReferenceExpression EventRef(string name)
        {
            return new CodeEventReferenceExpression(new CodeThisReferenceExpression(), name);
        }

        public static CodeEventReferenceExpression EventRef(CodeExpression target, string name)
        {
            return new CodeEventReferenceExpression(target, name);
        }

        public static CodeMethodInvokeExpression MethodInvoke(string name, CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), name, parameters);
        }

        public static CodeMethodInvokeExpression MethodInvoke(CodeExpression target, string name, CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(target, name, parameters);
        }

        public static CodeMethodInvokeExpression BaseMethodInvoke(string name, CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), name, parameters);
        }

        public static CodeMethodInvokeExpression StaticMethodInvoke(string classname, string name, CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(TypeRef(classname)), name, parameters);
        }

        public static CodeMethodInvokeExpression GlobalMethodInvoke(string name, CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(null, name, parameters);
        }

        public static CodeBinaryOperatorExpression Equals(CodeExpression exp1, CodeExpression exp2)
        {
            return new CodeBinaryOperatorExpression(exp1, CodeBinaryOperatorType.IdentityEquality, exp2);
        }

        public static CodeBinaryOperatorExpression Inequals(CodeExpression exp1, CodeExpression exp2)
        {
            return new CodeBinaryOperatorExpression(exp1, CodeBinaryOperatorType.IdentityInequality, exp2);
        }

        public static CodeTypeReferenceExpression TypeRefExp(Type t)
        {
            return new CodeTypeReferenceExpression(t);
        }

        public static CodeTypeReferenceExpression TypeRefExp(string name)
        {
            return new CodeTypeReferenceExpression(name);
        }

        public static CodeExpressionStatement Eval(CodeExpression exp)
        {
            return new CodeExpressionStatement(exp);
        }

        public static CodeAssignStatement Let(CodeExpression exp, CodeExpression value)
        {
            return new CodeAssignStatement(exp, value);
        }

        public static CodeMethodReturnStatement Return(CodeExpression exp)
        {
            return new CodeMethodReturnStatement(exp);
        }

        public static CodeVariableDeclarationStatement VarDecl(Type t, string name, CodeExpression init)
        {
            return new CodeVariableDeclarationStatement(t, name, init);
        }

        public static CodeVariableDeclarationStatement VarDecl(string t, string name, CodeExpression init)
        {
            return new CodeVariableDeclarationStatement(t, name, init);
        }

        public static CodeCommentStatement Comment(string comment)
        {
            return new CodeCommentStatement(comment);
        }

        public static CodeThrowExceptionStatement Throw(CodeExpression exp)
        {
            return new CodeThrowExceptionStatement(exp);
        }

        public static CodeStatement[] MakeCodeStatementArray(ArrayList AStatementList)
        {
            Int32 counter;

            CodeStatement[] Result = new CodeStatement[AStatementList.Count];

            for (counter = 0; counter <= AStatementList.Count - 1; counter += 1)
            {
                Result[counter] = (CodeStatement)AStatementList[counter];
            }

            return Result;
        }
    }
}