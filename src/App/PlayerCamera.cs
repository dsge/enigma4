using Godot;


namespace App
{
    public class PlayerCamera : Camera {

        protected Vector3 temporaryTranslation = Vector3.Zero;

        public void setTemporaryTranslation(Vector3 temporaryTranslation) {
            this.clearTemporaryTranslation();
            this.temporaryTranslation = temporaryTranslation;
            this.Translation = this.Translation + this.temporaryTranslation;
        }

        public void clearTemporaryTranslation() {
            this.Translation = this.Translation - this.temporaryTranslation;
            this.temporaryTranslation = Vector3.Zero;
        }

    }
}
