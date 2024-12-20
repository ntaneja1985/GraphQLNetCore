using GraphQL;
using GraphQL.Types;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using GraphQLProject.Type;

namespace GraphQLProject.Mutation
{
    public class CategoryMutation : ObjectGraphType
    {
        public CategoryMutation(ICategoryRepository categoryRepository) 
        {
            Field<CategoryType>("AddCategory").Arguments(new QueryArguments(new QueryArgument<CategoryInputType> { Name = "category" })).Resolve(context =>
            {
                return categoryRepository.AddCategory(context.GetArgument<Category>("category")).GetAwaiter().GetResult();
            });

            Field<CategoryType>("UpdateCategory").Arguments(new QueryArguments(new QueryArgument<IntGraphType> { Name = "categoryId" }, new QueryArgument<CategoryInputType> { Name = "category" })).Resolve(context =>
            {
                return categoryRepository.UpdateCategory(context.GetArgument<int>("categoryId"),context.GetArgument<Category>("category")).GetAwaiter().GetResult();
            });

            Field<BooleanGraphType>("DeleteCategory").Arguments(new QueryArguments(new QueryArgument<IntGraphType> { Name = "categoryId" })).Resolve(context =>
            {
                categoryRepository.DeleteCategory(context.GetArgument<int>("categoryId")).GetAwaiter().GetResult();
                 return true;
            });


        }
    }
}
