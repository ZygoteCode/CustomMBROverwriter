[BITS 16]
[ORG 7C00h]

JMP main

main:
    XOR AX, AX
    MOV DS, AX
    CLD
    MOV AX, 0012H
    INT 10H
    MOV SI, string
    MOV BL, %color%
    CALL printstr
    JMP  $
	
printstr:
    MOV BH, 0
	
print:
	LODSB
	CMP AL, 0
	JE done
	MOV AH, 0EH
	INT 10H
	JMP print
	
done:
    RET

string DB "%str%"
TIMES 510 - ($-$$) DB 0
DW 0AA55h