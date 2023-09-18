using FilmReview.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FilmReview.Filters
{
    public class SensitiveWordFilterAttribute : ActionFilterAttribute
    {

        private readonly string[] sensitiveWords;
        public SensitiveWordFilterAttribute()
        {
            string filePath = "./Filters/sw.txt";
            sensitiveWords = File.ReadAllLines(filePath)
                                .Where(line => !string.IsNullOrWhiteSpace(line))
                                .Select(line => line.Trim())
                                .ToArray();
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 在执行控制器方法之前进行敏感词过滤
            if (context.ActionArguments.TryGetValue("reviewDto", out var reviewDtoObject) && reviewDtoObject is ReviewDto reviewDto)
            {
                // 在 reviewDto 中检查敏感词并进行过滤
                if (FilterSensitiveWords(reviewDto.Content))
                {
                    context.Result = new BadRequestObjectResult("评论包含敏感词");
                    return;
                }

            }

            // 继续执行下一个 Action Filter 或控制器方法
            await next();
        }

        private bool FilterSensitiveWords(string input)
        {
            foreach (var sensitiveWord in sensitiveWords)
            {
                // 在 reviewDto 中检查评论中是否包含敏感词
                if (input.Contains(sensitiveWord))
                {
                    return true;
                }

            }
            return false;
        }

    }}
