namespace Studex.Domain.Dto;

public class LectureDto
{
    public string Name { get; set; } = default!;
    public string Topic { get; set; } = default!;
    public string? PdfFilePath { get; set; } = default!;
}