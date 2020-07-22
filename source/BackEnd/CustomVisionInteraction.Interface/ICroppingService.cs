namespace CustomVisionInteraction.Interface
{
    public interface ICroppingService
    {
        byte[] CropImage(byte[] image, IBoundingBox boundingBox);
    }
}
