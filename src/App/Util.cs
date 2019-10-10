using Godot;

namespace App
{
    public class Util {


        /**
         * load image files without properly importing them
         *
         * properly imported textures should be loaded with:
         * (Texture)GD.Load("res://static/foo.png")
         */
        public static Texture imageToTexture(string path) {
            var img = new Image();
            var ret = new ImageTexture();
            img.Load(path);
            ret.CreateFromImage(img);
            return ret;
        }
    }
}
