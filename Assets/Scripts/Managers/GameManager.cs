using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Managers;
using Inventory;
using System.IO;

[RequireComponent(typeof(SettingsManager))]
[RequireComponent(typeof(ItemManager))]
[RequireComponent(typeof(EntityManager))]
public class GameManager : MonoBehaviour {
    public static GameManager Instance {get; private set;}

    private SettingsManager settings;
    private ItemManager items;
    private EntityManager entities;

    private void Awake() {
        Instance = this;
        this.settings = GetComponent<SettingsManager>();
        this.items = GetComponent<ItemManager>();
        this.entities = GetComponent<EntityManager>();
    }

    public void InstantiateArrow(Stats.ItemStats _arrow, Vector3 _position, Vector2 _direction, uint _speed) {
        if(this.entities.GetEntity("Arrow", out Stats.EntityStats _entity)) {
            int rot = 0;
            if(_direction == Vector2.up)        rot = -45;
            else if(_direction == Vector2.down || _direction == Vector2.zero) rot = 135;
            else if(_direction == Vector2.left) rot = 45;
            else if(_direction == Vector2.right) rot = -135;
            else if(_direction == (Vector2.up + Vector2.left)) rot = 0;
            else if(_direction == (Vector2.up + Vector2.right)) rot = 90;
            else if(_direction == (Vector2.down + Vector2.left)) rot = -90;
            else if(_direction == (Vector2.down + Vector2.right)) rot = 180;

            GameObject arrow = Instantiate(
                _entity.gameObject,
                _position + new Vector3(
                    _direction.x,
                    _direction.y,
                    0
                ) * 0.5f,
                Quaternion.Euler(new Vector3(0, 0, rot))
            );
            arrow.GetComponent<SpriteRenderer>().sprite = this.items.GetItem(_arrow).GetComponent<SpriteRenderer>().sprite;
            arrow.GetComponent<Controller.ArrowController>().MoveDirection(_direction == Vector2.zero ? Vector2.down : _direction);
            arrow.GetComponent<Stats.EntityStats>().Speed = _speed;
            arrow.GetComponent<Inventory.Inventory>().amount[ItemManager.Instance.Items.FindIndex(x => x.Id == _arrow.Id)] = 1;
            arrow.GetComponent<Inventory.Inventory>().equiped[ItemManager.Instance.Items.FindIndex(x => x.Id == _arrow.Id)] = true;

            this.entities.CurrentEntities.Add(arrow.GetComponent<Stats.EntityStats>());
        }
    }

    public Stats.EntityStats InstantiateItem(Stats.ItemStats _item, Vector3 _position) {
        Stats.EntityStats item = Instantiate(
            ItemManager.Instance.GetItem(_item).gameObject,
            _position,
            Quaternion.Euler(Vector3.zero)
        ).GetComponent<Stats.EntityStats>();

        this.entities.CurrentEntities.Add(item);
        return item;
    }

    public Stats.EntityStats InstantiateEntity(Stats.EntityStats _entity, Vector3 _position) {
        if(this.entities.GetEntity(_entity.Name, out Stats.EntityStats entity)) {
            entity = Instantiate(
                entity.gameObject,
                _position,
                Quaternion.Euler(Vector3.zero)
            ).GetComponent<Stats.EntityStats>();

            this.entities.CurrentEntities.Add(entity);
            return entity;
        }
        return null;
    }

    public void Execution(bool _active) {
        this.entities.CurrentEntities.ForEach(x => {
            if(x.TryGetComponent<Controller.PlayerController>(out Controller.PlayerController pCon))
                pCon.gameObject.SetActive(_active);
            if(x.TryGetComponent<Controller.EntityBaseController>(out Controller.EntityBaseController ebCon))
                ebCon.gameObject.SetActive(_active);
            if(x.TryGetComponent<Controller.ArrowController>(out Controller.ArrowController aCon))
                aCon.gameObject.SetActive(_active);
            if(x.TryGetComponent<Controller.EnemyController>(out Controller.EnemyController eCon))
                eCon.gameObject.SetActive(_active);
        });
    }

    public void Spawn() {
        InstantiateEntity(this.entities.Entities.First(x => x.TryGetComponent<Controller.PlayerController>(out _)), new Vector3(5, 0, 0));
        this.entities.Entities.Where(x => x.TryGetComponent<Controller.EnemyController>(out _)).ToList().ForEach(x => InstantiateEntity(x, new Vector3(Random.Range(-5, 6), Random.Range(-5, 6), 0)));
        this.entities.Entities.Where(x => x.TryGetComponent<Controller.NPCController>(out _)).ToList().ForEach(x => InstantiateEntity(x, new Vector3(10, 5, 0)));
    }

    public void Unspawn() {
        Stats.EntityStats[] temp = new Stats.EntityStats[this.entities.CurrentEntities.Count];
        this.entities.CurrentEntities.CopyTo(temp);
        temp.ToList().ForEach(x => {
            if (x.TryGetComponent<Controller.PlayerController>(out Controller.PlayerController pCon))
                pCon.Kill();
            if (x.TryGetComponent<Controller.EntityBaseController>(out Controller.EntityBaseController ebCon))
                ebCon.Kill();
            if (x.TryGetComponent<Controller.ArrowController>(out Controller.ArrowController aCon))
                aCon.Kill();
            if (x.TryGetComponent<Controller.EnemyController>(out Controller.EnemyController eCon))
                eCon.Kill();
        });
    }

    public enum ControllerType {
        Base = 0,
        Player = 1,
        Enemy = 2,
        Arrow = 3
    }

    public void Load() {
        using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.dat"))) {
            int currentEntAmount = reader.ReadInt32();
            for(int i = 0; i < currentEntAmount; i++) {
                var pos = new Vector3(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    0
                );

                string name = reader.ReadString();
                Stats.EntityStats entity = null;
                if(this.entities.GetEntity(name, out Stats.EntityStats _entity))
                    entity = InstantiateEntity(
                        _entity,
                        pos
                    );
                else
                    entity = InstantiateItem(
                        ItemManager.Instance.Items.First(x => x.Name == name),
                        pos
                    );

                entity.GetComponent<Stats.EntityStats>().OnLoad(reader);

                int type = reader.ReadInt32();
                switch (type) {
                    case (int) ControllerType.Player:
                        entity.GetComponent<Controller.PlayerController>().OnLoad(reader);
                        break;
                    case (int) ControllerType.Enemy:
                        entity.GetComponent<Controller.EnemyController>().OnLoad(reader);
                        break;
                    case (int) ControllerType.Base:
                        entity.GetComponent<Controller.EntityBaseController>().OnLoad(reader);
                        break;
                    case (int) ControllerType.Arrow:
                        entity.GetComponent<Controller.ArrowController>().OnLoad(reader);
                        break;
                }

                entity.GetComponent<Inventory.Inventory>().OnLoad(reader);
            }
        }
    }
    public void Save() {
        using(BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.dat"))) {
            writer.Write(this.entities.CurrentEntities.Count);
            this.entities.CurrentEntities.ForEach(x => {
                // Position
                writer.Write(x.transform.position.x);
                writer.Write(x.transform.position.y);

                // Entity Stats
                Stats.EntityStats entStats = x.GetComponent<Stats.EntityStats>();
                writer.Write(entStats.Name);

                entStats.OnSave(writer);

                // Controller Data
                if (x.TryGetComponent<Controller.PlayerController>(out Controller.PlayerController pCon)) {
                    writer.Write((int) ControllerType.Player);

                    pCon.OnSave(writer);
                } else if (x.TryGetComponent<Controller.ArrowController>(out Controller.ArrowController aCon)) {
                    writer.Write((int) ControllerType.Arrow);

                    aCon.OnSave(writer);
                } else if (x.TryGetComponent<Controller.EnemyController>(out Controller.EnemyController eCon)) {
                    writer.Write((int) ControllerType.Enemy);

                    eCon.OnSave(writer);
                } else if (x.TryGetComponent<Controller.EntityBaseController>(out Controller.EntityBaseController ebCon)) {
                    writer.Write((int)ControllerType.Base);

                    ebCon.OnSave(writer);
                }

                // Inventory
                x.GetComponent<Inventory.Inventory>().OnSave(writer);
            });
        }
    }
}

public interface ISave {
    void OnSave(BinaryWriter _writer);
    void OnLoad(BinaryReader _reader);
}
