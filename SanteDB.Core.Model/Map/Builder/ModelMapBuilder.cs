using SanteDB.Core.Model.Resources;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SanteDB.Core.Model.Map.Builder
{
    /// <summary>
    /// Model map builder
    /// </summary>
    /// <remarks>
    /// The old version of the model mapping class used reflection which was extremely inefficient for mapping 
    /// large sets of data. As part of the nuado refactoring, the model mapping is now handled by programmatically 
    /// generated model mapping classes which contain explicit instructions on how to map the data from the source 
    /// model into the target model
    /// </remarks>
    public class ModelMapBuilder
    {
        private static readonly CodePrimitiveExpression s_true = new CodePrimitiveExpression(true);
        private static readonly CodePrimitiveExpression s_false = new CodePrimitiveExpression(false);
        private static readonly CodePrimitiveExpression s_null = new CodePrimitiveExpression(null);
        private static readonly CodeThisReferenceExpression s_this = new CodeThisReferenceExpression();

        private static readonly CodeFieldReferenceExpression s_mapperContext = new CodeFieldReferenceExpression(s_this, "m_mapper");
        private static readonly CodeVariableReferenceExpression s_retVal = new CodeVariableReferenceExpression("_retVal");
        private static readonly CodeTypeReference s_tIdentifiedData = new CodeTypeReference(typeof(IdentifiedData));
        private static readonly CodeTypeReference s_tType = new CodeTypeReference(typeof(Type));



        /// <summary>
        /// Creates a String.Format() expression
        /// </summary>
        private CodeExpression CreateStringFormatExpression(string formatString, params CodeExpression[] parms)
        {
            var retVal = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(String)), "Format"), new CodePrimitiveExpression(formatString));
            foreach (var p in parms)
                retVal.Parameters.Add(p);
            return retVal;
        }

        /// <summary>
        /// Create a instance cast expression
        /// </summary>
        private CodeStatement CreateCastTryCatch(Type toType, CodeExpression targetObject, CodeExpression sourceObject, CodeStatement failExpression)
        {
            return new CodeTryCatchFinallyStatement(
                new CodeStatement[] {
                    new CodeAssignStatement(targetObject, new CodeCastExpression(new CodeTypeReference(toType), sourceObject)),
                },
                new CodeCatchClause[] {

                new CodeCatchClause("e", new CodeTypeReference(typeof(Exception)),
                    failExpression)
                });
        }

        /// <summary>
        /// Create a simaple method call expression
        /// </summary>
        private CodeExpression CreateSimpleMethodCallExpression(CodeExpression target, String methodName, params Object[] simpleParameters)
        {
            var retVal = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(target, methodName));
            foreach (var p in simpleParameters)
                retVal.Parameters.Add(new CodePrimitiveExpression(p));
            return retVal;
        }

        /// <summary>
        /// Create a simaple method call expression
        /// </summary>
        private CodeExpression CreateMethodCallExpression(CodeExpression target, String methodName, params CodeExpression[] parameters)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(target, methodName), parameters);
        }

        /// <summary>
        /// Create ConvertExpression
        /// </summary>
        private CodeExpression CreateConvertExpression(String methodName, CodeExpression parameter)
        {
            return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Convert)), methodName, parameter);
        }


        /// <summary>
        /// Create an equals statement
        /// </summary>
        private CodeMethodInvokeExpression CreateValueEqualsStatement(CodeExpression left, CodeExpression right)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(left, "Equals"), right);
        }

        /// <summary>
        /// Create a throw exception
        /// </summary>
        /// <param name="exceptionType">The type of exception to be thrown</param>
        /// <param name="exceptionArguments">The arguments to the throw</param>
        /// <returns>The exception statement</returns>
        private CodeThrowExceptionStatement CreateThrowException(Type exceptionType, params CodeExpression[] exceptionArguments)
        {
            return new CodeThrowExceptionStatement(new CodeObjectCreateExpression(exceptionType, exceptionArguments));
        }

        /// <summary>
        /// Creates a call to GetType()
        /// </summary>
        private CodeExpression CreateGetTypeExpression(CodeExpression _object)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(_object, "GetType"));
        }

        /// <summary>
        /// Create type reference
        /// </summary>
        private CodeTypeReference CreateInternalModelMapInterface(Type sourceType, Type targetType)
        {
            var retVal = new CodeTypeReference("IModelMapperInternal");
            retVal.TypeArguments.Add(new CodeTypeReference(sourceType));
            retVal.TypeArguments.Add(new CodeTypeReference(targetType));
            return retVal;
        }

        /// <summary>
        /// Create a code namespace for the specified assembly
        /// </summary>
        public CodeNamespace CreateCodeNamespace(ModelMap map, String name)
        {
            CodeNamespace retVal = new CodeNamespace($"SanteDB.Core.Model.Map.{name}");

            retVal.Types.Add(this.CreateInternalModelMapInterface());

            foreach (var t in map.Class)
            {
                var ctdecl = this.CreateMapper(t);
                if (ctdecl != null)
                    retVal.Types.Add(ctdecl);
            }
            return retVal;
        }

        /// <summary>
        /// Create 
        /// </summary>
        public CodeTypeDeclaration CreateInternalModelMapInterface()
        {
            CodeTypeDeclaration retVal = new CodeTypeDeclaration($"IModelMapperInternal");

            var tsource = new CodeTypeParameter("TSource");
            var ttarget = new CodeTypeParameter("TTarget");
            retVal.TypeParameters.Add(tsource);
            retVal.TypeParameters.Add(ttarget);
            retVal.BaseTypes.Add(typeof(IModelMapper));
            var mapMethodDef = new CodeMemberMethod()
            {
                Name = "MapToTarget",
                ReturnType = new CodeTypeReference(ttarget)
            };
            mapMethodDef.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(tsource), "source"));
            mapMethodDef.Parameters.Add(new CodeParameterDeclarationExpression(typeof(HashSet<Guid>), "stack"));
            mapMethodDef.Attributes = MemberAttributes.Public;
            retVal.Members.Add(mapMethodDef);

            mapMethodDef = new CodeMemberMethod()
            {
                Name = "MapToSource",
                ReturnType = new CodeTypeReference(tsource)
            };
            mapMethodDef.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(ttarget), "target"));
            mapMethodDef.Parameters.Add(new CodeParameterDeclarationExpression(typeof(HashSet<Guid>), "stack"));
            retVal.Members.Add(mapMethodDef);

            retVal.IsInterface = true;
            retVal.IsClass = false;

            retVal.TypeAttributes = TypeAttributes.NotPublic | TypeAttributes.Interface;
            return retVal;
        }


        /// <summary>
        /// Creates a view model serializer
        /// </summary>
        public CodeTypeDeclaration CreateMapper(ClassMap map)
        {

            // Cannot process this type
            if (map.DomainType == null || map.ModelType == null)
                return null;

            // Generate the type definition
            string className = $"{map.ModelType.Name}To{map.DomainType.Name}ModelMapper";
            CodeTypeDeclaration retVal = new CodeTypeDeclaration(className);
            retVal.IsClass = true;
            retVal.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            retVal.BaseTypes.Add(typeof(IModelMapper));
            retVal.BaseTypes.Add(typeof(IModelMapper<,>).MakeGenericType(map.ModelType, map.DomainType));
            retVal.BaseTypes.Add(this.CreateInternalModelMapInterface(map.ModelType, map.DomainType));

            retVal.Members.Add(new CodeMemberField(typeof(ModelMapper), "m_mapper") { Attributes = MemberAttributes.Private });
            retVal.Members.Add(this.CreateInitializorConstructor(className));
            // Add methods
            retVal.Members.Add(this.CreateTypeGetProperty("SourceType", map.ModelType));
            retVal.Members.Add(this.CreateTypeGetProperty("TargetType", map.DomainType));

            retVal.Members.Add(this.CreateTypedMapMethod("MapToSource", map.DomainType, map.ModelType));
            retVal.Members.Add(this.CreateTypedMapMethod("MapToTarget", map.ModelType, map.DomainType));
            retVal.Members.Add(this.CreateObjectMapMethod("MapToSource", map.DomainType));
            retVal.Members.Add(this.CreateObjectMapMethod("MapToTarget", map.ModelType));
            //retVal.Members.Add(this.CreateMapToTargetGenInternal(map.ModelType, map.DomainType));
            //retVal.Members.Add(this.CreateMapToSourceGenInternal(map.ModelType, map.DomainType));

            return retVal;
        }

        /// <summary>
        /// Create a constructor which accepts a <see cref="ModelMapper"/>
        /// </summary>
        /// <param name="className">The name of the constructor class</param>
        private CodeConstructor CreateInitializorConstructor(string className)
        {
            var retVal = new CodeConstructor()
            {
                Name = className,
                Attributes = MemberAttributes.Public,
            };
            retVal.Parameters.Add(new CodeParameterDeclarationExpression(typeof(ModelMapper), "mapper"));
            retVal.Statements.Add(new CodeAssignStatement(s_mapperContext, new CodeVariableReferenceExpression("mapper")));
            return retVal;
        }

        /// <summary>
        /// Creates a generic typed form of MapToX method
        /// </summary>
        /// <param name="methodName">The name of the method to generate</param>
        /// <param name="parameterType">The type of parameter to accept</param>
        /// <param name="returnType">The type of parameter to return</param>
        /// <returns>The code type member</returns>
        private CodeTypeMember CreateTypedMapMethod(string methodName, Type parameterType, Type returnType)
        {
            var retVal = new CodeMemberMethod()
            {
                Name = methodName,
                ReturnType = new CodeTypeReference(returnType),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            retVal.Parameters.Add(new CodeParameterDeclarationExpression(parameterType, "o"));
            var _o = new CodeVariableReferenceExpression("o");

            retVal.Statements.Add(this.CreateArgumentNullCheckStatement(_o));
            retVal.Statements.Add(new CodeVariableDeclarationStatement(typeof(HashSet<Guid>), "keySet", new CodeObjectCreateExpression(new CodeTypeReference(typeof(HashSet<Guid>)))));
            var _keySet = new CodeVariableReferenceExpression("keySet");

            // Call and return
            retVal.Statements.Add(new CodeMethodReturnStatement(this.CreateMethodCallExpression(s_this, methodName, _o, _keySet)));

            return retVal;
        }

        /// <summary>
        /// Creates a non-generic form of the MapToX method, casting to appropriate <paramref name="parameterType"/>
        /// </summary>
        /// <param name="methodName">The name of the method to generate</param>
        /// <param name="parameterType">The type of parameter to cast</param>
        /// <returns>The code type member</returns>
        private CodeTypeMember CreateObjectMapMethod(string methodName, Type parameterType)
        {
            var retVal = new CodeMemberMethod()
            {
                Name = methodName,
                ReturnType = new CodeTypeReference(typeof(Object)),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            retVal.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Object), "o"));
            var _o = new CodeVariableReferenceExpression("o");

            retVal.Statements.Add(this.CreateArgumentNullCheckStatement(_o));
            retVal.Statements.Add(new CodeVariableDeclarationStatement(parameterType, "instance"));
            var _instance = new CodeVariableReferenceExpression("instance");
            
            // Cast the object to the correct type
            retVal.Statements.Add(this.CreateCastTryCatch(parameterType, _instance, _o, this.CreateThrowException(typeof(ArgumentException), new CodePrimitiveExpression("instance"), this.CreateStringFormatExpression(ErrorMessages.ERR_MAP_INVALID_TYPE, this.CreateGetTypeExpression(_o)), new CodeVariableReferenceExpression("e"))));

            // Call and return
            retVal.Statements.Add(new CodeMethodReturnStatement(this.CreateMethodCallExpression(s_this, methodName, _instance)));

            return retVal;
            
        }

        /// <summary>
        /// Create null check for argument <paramref name="argumentToCheck"/>
        /// </summary>
        /// <param name="argumentToCheck">The argument to check</param>
        /// <returns>The conditional statement</returns>
        /// <remarks>Checks that <paramref name="argumentToCheck"/> is null, and if so, then throws an argumentnullexception</remarks>
        private CodeConditionStatement CreateArgumentNullCheckStatement(CodeVariableReferenceExpression argumentToCheck)
        {
            return new CodeConditionStatement(new CodeBinaryOperatorExpression(argumentToCheck, CodeBinaryOperatorType.IdentityEquality, s_null), this.CreateThrowException(typeof(ArgumentNullException), new CodePrimitiveExpression(argumentToCheck.VariableName), this.CreateStringFormatExpression(ErrorMessages.ERR_ARGUMENT_NULL, new CodePrimitiveExpression(argumentToCheck.VariableName))));
        }

        /// <summary>
        /// Create a ToString call on <paramref name="sourceObject"/>
        /// </summary>
        /// <param name="sourceObject">The object to call ToString on</param>
        /// <returns>The ToString expression</returns>
        private CodeMethodInvokeExpression CreateToStringExpression(CodeExpression sourceObject)
        {
            return new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(sourceObject, "ToString"));
        }

        /// <summary>
        /// Create one of the type properties on <see cref="IModelMapper{TSource, TTarget}"/>
        /// </summary>
        /// <param name="propertyName">The name of the property in the interface</param>
        /// <param name="type">The type to return from the property</param>
        /// <returns>The type member to be attached to the source property</returns>
        private CodeTypeMember CreateTypeGetProperty(String propertyName, Type type)
        {
            var retVal = new CodeMemberProperty()
            {
                Name = propertyName,
                Type = s_tType,
                HasGet = true,
                HasSet = false,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            retVal.GetStatements.Add(new CodeMethodReturnStatement(new CodeTypeOfExpression(type)));
            return retVal;
        }

    }
}
