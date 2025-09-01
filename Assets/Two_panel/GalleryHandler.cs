using UnityEngine;

[System.Serializable]
public class ImagePrefabMapping
{
    public string imageName;    // Name of the reference image (without extension)
    public GameObject prefab;   // Prefab to spawn
}

public class GalleryHandler : MonoBehaviour
{
    public ImagePrefabMapping[] mappings;  // Set in Inspector
    public Transform spawnPoint;           // Where the prefab will appear

    public void OpenGallery()
    {
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                foreach (var map in mappings)
                {
                    if (map.imageName == fileName)
                    {
                        Instantiate(map.prefab, spawnPoint.position, Quaternion.identity);
                        Debug.Log("Prefab spawned for: " + fileName);
                        return;
                    }
                }

                Debug.Log("No prefab found for: " + fileName);
            }
        }, "Select an image", "image/*");
    }
}
