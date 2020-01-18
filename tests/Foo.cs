using System;
using NUnit.Framework;

namespace Tests
{
    public class FooTest
    {
        [Test]
        public void sampleTest()
        {
            var instance = new App.PlayerCamera();

            Assert.Zero(instance.temporaryTranslation.x);

            instance.setTemporaryTranslation(new Godot.Vector3(4f, 3f, 2f));

            Assert.NotZero(instance.temporaryTranslation.x);

            instance.clearTemporaryTranslation();

            Assert.Zero(instance.temporaryTranslation.x);
        }
    }
}
