using Godot;

namespace App
{
    public class Util {


        public static Texture imageToTexture(string path) {
            var img = new Image();
            var ret = new ImageTexture();
            img.Load(path);
            ret.CreateFromImage(img);
            return ret;
        }
    }
}
