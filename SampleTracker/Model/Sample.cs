namespace SampleTracker.Model;

public class Sample
{
    private int id;
    private string? filename;
    private string? sha256;
    private string? note;

    public int Id
    {
        get => id; 
        set => id = value;
    }

    public string? FileName
    {
        get => filename; 
        set => filename = value;
    }
    public string? Sha256
    {
        get => sha256;
        set => sha256 = value;
    }

    public string? Note
    {
        get => note;
        set => note = value;
    }
}