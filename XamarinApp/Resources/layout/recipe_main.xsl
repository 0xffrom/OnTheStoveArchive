<?xml version="1.0" encoding="utf-8"?>
<LinearLayout
        xmlns:android="http://schemas.android.com/apk/res/android"
        xmlns:app="http://schemas.android.com/apk/res-auto"
        xmlns:tools="http://schemas.android.com/tools"
        android:layout_width="match_parent"
        android:layout_height="match_parent"
        android:orientation="vertical">

    <LinearLayout
            android:orientation="horizontal"
            android:id="@+id/rectangle_2"
            android:layout_width="match_parent"
            android:layout_height="64dp"
            android:layout_alignParentLeft="true"
            android:layout_alignParentTop="true"
            android:background="@drawable/rectangle_2">
        <Button
                android:layout_marginTop="6dp"
                android:layout_width="10.37dp"
                android:layout_height="18.73dp"
                android:background="@drawable/icon_back"
                android:id="@+id/button2"
                android:layout_marginRight="10dp"
                android:layout_marginLeft="16dp"/>
        <TextView
                android:gravity="center_horizontal"
                android:text="Это самое сочное оливье"
                android:layout_height="wrap_content"
                android:layout_width="wrap_content"
                android:textColor="#FCFCFC"
                android:textSize="21dp"
                android:layout_marginTop="@dimen/fab_margin"


        />

    </LinearLayout>
    <LinearLayout
            android:orientation="vertical"
            android:layout_width="match_parent"
            android:layout_height="match_parent">
        <GridLayout 
                android:layout_marginTop="17dp"
                android:layout_width="match_parent"
                android:layout_height="28dp">
            <Button 
                    android:layout_column="0"
                    android:text="Описание"
                    android:layout_marginLeft="21dp"
                    android:textSize="14dp"
                    android:gravity="top"
                    android:layout_width="82dp" 
                    android:layout_height="20dp"/>
            <Button
                    android:layout_column="1"
                    android:text="Ингридиенты"
                    android:layout_marginLeft="21dp"
                    android:textSize="14dp"
                    android:gravity="top"
                    android:layout_width="95dp"
                    android:layout_height="20dp"/>
            <Button
                    android:layout_column="2"
                    android:text="Рецепт"
                    android:layout_gravity="left"
                    android:layout_marginLeft="21dp"
                    android:textSize="14dp"
                    android:gravity="top"
                    android:layout_width="82dp"
                    android:layout_height="20dp"/>
        </GridLayout>
    </LinearLayout>


</LinearLayout>
