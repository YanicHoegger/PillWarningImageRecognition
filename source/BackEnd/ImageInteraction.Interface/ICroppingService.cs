namespace ImageInteraction.Interface
{
    public interface ICroppingService
    {
        byte[] CropImage(byte[] image, IBoundingBox boundingBox);
    }
}
