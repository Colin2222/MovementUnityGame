using System.Collections;
using System.Collections.Generic;

public interface IMenuState
{
    void MenuUp();
    void MenuDown();
    void MenuLeft();
    void MenuRight();
    void MenuSelect();
    void MenuDrop();
    PState MenuExit();
}
