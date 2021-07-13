    using System;
    using UnityEngine;

    [Serializable]
    public struct AssessStructure
    {
        public string assignmentRecordName;
        public assignmentRecordOperation assignmentRecordOperation;
        public string fecha;
        public string hora;
        //public assignmentRecordTest assignmentRecordTest;
    }

    [Serializable]
    public struct assignmentRecordOperation
    {
        public panelControl panelControl;
        public tensarBanda tensarBanda;
        public int tiempo;
    }
    
    [Serializable]
    public struct panelControl
    {
        public bool arranque;
        public bool paro;
        public bool velocidadAlta;
        public bool velocidadBaja;
    }

    [Serializable]
    public struct tensarBanda
    {
        public string pasos;
    }

    /*[Serializable]
    public struct assignmentRecordTest
    {
        public int tiempo;
        public string respuestas;
        public double calificacion;
        public string intentos;
    }*/

