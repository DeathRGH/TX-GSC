# T7 GSC Injection Example

In this example I will show a way to inject a compiled gsc menu without using any sdk.<br>
This is achieved by simply hex editing the eboot, making this a viable option for test and devkits.<br>
The compiled gsc file is read from the root of the pkg or hostapp directory.<br><br>
The addresses used in this example are the file addresses so you can open your eboot.bin in a hex editor like HxD and edit it without any calculation required.
<br><br>
The compiled file to be used with this example can be found [here](https://github.com/DeathRGH/TX-GSC/blob/master/T7Files/compiled.gsc).


## Required Strings
First things first we need to find a good function we can replace that the game does not need.<br>
A pretty good one that is also fairly big to inject a lot of custom instrcutions is `CG_DrawBigFPS`.<br>
<br>
In the 1.33 eboot.bin this function is located at `0x5F2F90` in the file.

1. Return Function<br>
To prevent the game from calling this function we will write a simple return at the start.<br>
This is required because we will write our string data there.<br>
`5F2F90   C3   ret`

2. Writing The Required Strings<br>
The strings we require are:
- "rb" (used for `fopen`)
- "/app0/compiled.gsc" (path to our custom compiled gsc file)
- "scripts/shared/duplicaterender_mgr.gsc" (path of the file we will replace)

We can inject starting at `0x5F2F8D` as the padding before the function can also be used.

Before:
```
        00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
5F2F80  .. .. .. .. .. .. .. .. .. .. .. .. .. 90 90 90   ................
5F2F90  55 48 89 E5 41 57 41 56 41 55 41 54 53 48 83 E4   UH‰åAWAVAUATSHƒä
5F2FA0  E0 48 81 EC E0 05 00 00 48 8B 1D 01 19 01 01 41   àH.ìà...H‹.....A
5F2FB0  89 FE 48 8B 03 48 89 84 24 C0 05 00 00 48 8B 3D   ‰þH‹.H‰„$À...H‹=
5F2FC0  84 F6 9E 02 E8 07 83 9C 00 85 C0                  „öž.è.ƒœ.…
```

After:
```
        00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
5F2F80  .. .. .. .. .. .. .. .. .. .. .. .. .. 72 62 00   .............rb.
5F2F90  C3 2F 61 70 70 30 2F 63 6F 6D 70 69 6C 65 64 2E   Ã/app0/compiled.
5F2FA0  67 73 63 00 73 63 72 69 70 74 73 2F 73 68 61 72   gsc.scripts/shar
5F2FB0  65 64 2F 64 75 70 6C 69 63 61 74 65 72 65 6E 64   ed/duplicaterend
5F2FC0  65 72 5F 6D 67 72 2E 67 73 63 00                  er_mgr.gsc.
```

<br><br>

## Entry Point
For our entry point we want to "hook" into the function that loads any gsc file with `DB_FindXAssetHeader`.<br>
To do this we jump to a region in memory where we write custom instructions to check which file is loaded.

```
773A0E   jmp 0x5F2FCB
```

<br><br>

## Writing The Injection Code

We need a total of 4 function that the game imports (file addresses):
```
0x121E780 // strcmp
0x121E820 // malloc
0x121E740 // fopen
0x121F140 // fread
```

### Pseudo Code
Here is some pseudo code to explain this example.<br>
As we chose a clever entry point, the file name string will still be in one of the registers.<br>
We can compare (`strcmp`) against that to see if the game is attempting to load `"scripts/shared/duplicaterender_mgr.gsc"`.<br>
If that is the case we will allocate (`malloc`) memory to write our custom gsc file to using `fopen` and `fread`.
Finally the address of the buffer is written to the location where the game expects the gsc file.
```c
if (!strcmp(#input_file_name, "scripts/shared/duplicaterender_mgr.gsc")) {
    char *buffer = (char *)malloc(0x461B0); // 0x461B0 = size of compiled gsc file + 1
    void *file = fopen("/app0/compiled.gsc", "rb");
    fread(buffer, 1, 0x461AF, file); // 0x461AF = size of compiled gsc file
    *(unsigned long long *)#input_file_address = buffer;
}
```

From this point onwards, all code will be injected at `0x5F2FCB` which is directly after our last string.<br><br>



@todo: add detailed flow of the code



<br><br>

## Final Code
Entry Point
```
773A0E   jmp 0x5F2FCB
```
```
        00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
773A00  .. .. .. .. .. .. .. .. .. .. .. .. .. .. E9 B8
773A10  F5 E7 FF
```
Injection
```
5F2F8D   .string "rb"
5F2F90   ret
5F2F91   .string "/app0/compiled.gsc"
5F2FA4   .string "scripts/shared/duplicaterender_mgr.gsc"
5F2FCB   mov eax, 0xFFFFFFFE
5F2FD0   test rcx, rcx
5F2FD3   je 0x773F1B
5F2FD9   mov r12, rcx
5F2FDC   nop
5F2FDD   nop
5F2FDE   lea rdi, [rip - 0x41]     # "scripts/shared/duplicaterender_mgr.gsc"
5F2FE5   mov rsi, qword ptr [rcx]
5F2FE8   call 0x121E780            # call strcmp
5F2FED   cmp eax, 0
5F2FF0   mov eax, 0xFFFFFFFE
5F2FF5   mov rcx, r12
5F2FF8   jne 0x773A13
5F2FFE   mov rdi, 0x461B0          # (gsc file size + 1)
5F3005   call 0x121E820            # call malloc
5F300A   mov r14, rax
5F300D   lea rdi, [rip - 0x83]     # "/app0/compiled.gsc"
5F3014   lea rsi, [rip - 0x8E]     # "rb"
5F301B   call 0x121E740            # call fopen
5F3020   mov r15, rax
5F3023   mov rdi, r14
5F3026   mov rsi, 1
5F302D   mov rdx, 0x461AF          # (gsc file size)
5F3034   mov rcx, r15
5F3037   call 0x121F140            # call fread
5F303C   mov rsi, r14
5F303F   jmp 0x773A20
```
```
        00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F
5F2F80  .. .. .. .. .. .. .. .. .. .. .. .. .. 72 62 00  .............rb.
5F2F90  C3 2F 61 70 70 30 2F 63 6F 6D 70 69 6C 65 64 2E  Ã/app0/compiled.
5F2FA0  67 73 63 00 73 63 72 69 70 74 73 2F 73 68 61 72  gsc.scripts/shar
5F2FB0  65 64 2F 64 75 70 6C 69 63 61 74 65 72 65 6E 64  ed/duplicaterend
5F2FC0  65 72 5F 6D 67 72 2E 67 73 63 00 B8 FE FF FF FF  er_mgr.gsc.¸þÿÿÿ
5F2FD0  48 85 C9 0F 84 42 0F 18 00 49 89 CC 90 90 48 8D  H…É.„B...I‰Ì..H.
5F2FE0  3D BF FF FF FF 48 8B 31 E8 93 B7 C2 00 83 F8 00  =¿ÿÿÿH‹1è“·Â.ƒø.
5F2FF0  B8 FE FF FF FF 4C 89 E1 0F 85 15 0A 18 00 48 C7  ¸þÿÿÿL‰á.…....HÇ
5F3000  C7 B0 61 04 00 E8 16 B8 C2 00 49 89 C6 48 8D 3D  Ç°a..è.¸Â.I‰ÆH.=
5F3010  7D FF FF FF 48 8D 35 72 FF FF FF E8 20 B7 C2 00  }ÿÿÿH.5rÿÿÿè ·Â.
5F3020  49 89 C7 4C 89 F7 48 C7 C6 01 00 00 00 48 C7 C2  I‰ÇL‰÷HÇÆ....HÇÂ
5F3030  AF 61 04 00 4C 89 F9 E8 04 C1 C2 00 4C 89 F6 E9  ¯a..L‰ùè.ÁÂ.L‰öé
5F3040  DC 09 18 00                                      Ü...

```


<br><br><br>
## Final Notes
There is obviously other/easier ways to achive gsc injection by using custom libs and other methods as you will only have to touch the data segment.<br>
This writeup is however from the standpoint that you don't have access to any sdk, only the game itself.<br><br>
This could be improved by grabbing the file size of "/app0/compiled.gsc" dynamically with `fstat`.<br><br>
The same injection can be achieved on T8 or T9 allthough the logic for injection there is different.
