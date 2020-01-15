using Godot;

namespace App.Grid
{
    public class CellElectricity {

        public CellElectricity() {

        }

        public bool canProduce() {
            return true;
        }

        public bool canConsume() {
            return !this.canProduce();
        }

        public int availableProduction() {
            return 10;
        }

        public int currentProduction() {
            return 10;
        }

        public int maxConsumption() {
            return 10;
        }

        public int currentConsumption() {
            return 10;
        }

    }
}
