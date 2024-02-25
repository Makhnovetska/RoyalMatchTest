using UnityEngine;

namespace Controllers
{
    public class LocationItemController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer imgLocationItem;
        [SerializeField] private string imgName;
        
        public void SetActive(bool isActive)
        {
            if (isActive)
                SetSprite();
            else
                imgLocationItem.sprite = null;
        }

        private void SetSprite()
        {
            imgLocationItem.sprite = Resources.Load<Sprite>($"Images/LocationItems/{imgName}");
        }
    }
}